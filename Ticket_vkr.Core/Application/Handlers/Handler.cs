using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket_vkr.UI.Domain.Status;
using Ticket_vkr.UI.Entities;
using Ticket_vkr.UI.Ports;

namespace Ticket_vkr.UI.Application.Handlers;

public class Handler: IHandler
{
    private readonly IRepository _repository;

    public Handler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<Dictionary<string, string>> GetUserStatusesAsync(string deviceId)
    {
        var statuses = await _repository.GetByDeviceIdAsync(deviceId);

        return statuses.ToDictionary(s => s.SeriesId, s => s.Status);
    }

    public async Task<bool> UpdateStatusAsync(UpdateStatusRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.DeviceId) ||
                string.IsNullOrEmpty(request.SeriesId) ||
                string.IsNullOrEmpty(request.Status))
                return false;

            var existing = await _repository.GetByDeviceAndSeriesAsync(request.DeviceId, request.SeriesId);

            
            existing.Status = request.Status;
            existing.UpdatedAt = DateTime.UtcNow;

            await _repository.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<News>> GetNewsAsync(string sortBy = null, bool ascending = true)
    {
        if (string.IsNullOrEmpty(sortBy))
            return await _repository.GetAllAsync();

        return await _repository.GetSortedAsync(sortBy, ascending);
    }
    public async Task<object> GetPagedSeriesAsync(int page, int pageSize)
    {
        var (items, total) = await _repository.GetPagedAsync(page, pageSize);

        var series = items.Select(s => new
        {
            id = s.Id,
            title = s.Title,
            year = s.ReleaseYearStart,
            rating = s.Rating,
            image = s.PosterPath,
            genres = s.SeriesGenres?.Select(sg => sg.Genre?.Name).Where(g => g != null).ToList()
        });

        return new
        {
            series = series,
            hasMore = total > page * pageSize,
            total = total,
            currentPage = page,
            pageSize = pageSize
        };
    }
    public async Task<object> FilterByYearAsync(int from, int to)
    {
        var items = await _repository.FilterByYearAsync(from, to);

        return items.Select(s => new
        {
            id = s.Id,
            title = s.Title,
            year = s.ReleaseYearStart,
            rating = s.Rating,
            image = s.PosterPath,
            genres = s.SeriesGenres?
                .Where(sg => sg.Genre != null)
                .Select(sg => sg.Genre.Name)
                .ToList() ?? new List<string>()
        });
    }

    public async Task<object> FilterByGenreAsync(string genre)
    {
        var items = await _repository.FilterByGenreAsync(genre);

        return items.Select(s => new
        {
            id = s.Id,
            title = s.Title,
            year = s.ReleaseYearStart,
            rating = s.Rating,
            image = s.PosterPath,
            genres = s.SeriesGenres?
                .Where(sg => sg.Genre != null)
                .Select(sg => sg.Genre.Name)
                .ToList() ?? new List<string>()
        });
    }

    public async Task<object> FilterByYearAndGenreAsync(int from, int to, string genre)
    {
        var items = await _repository.FilterByYearAndGenreAsync(from, to, genre);

        return items.Select(s => new
        {
            id = s.Id,
            title = s.Title,
            year = s.ReleaseYearStart,
            rating = s.Rating,
            image = s.PosterPath,
            genres = s.SeriesGenres?
                .Where(sg => sg.Genre != null)
                .Select(sg => sg.Genre.Name)
                .ToList() ?? new List<string>()
        });
    }

    public async Task<AddSeriesResponse> AddSeriesAsync(AddSeriesRequest request)
    {
        var response = new AddSeriesResponse
        {
            Success = false,
            Errors = new Dictionary<string, List<string>>()
        };

        try
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(request);

            if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
            {
                foreach (var validationResult in validationResults)
                {
                    foreach (var memberName in validationResult.MemberNames)
                    {
                        if (!response.Errors.ContainsKey(memberName))
                            response.Errors[memberName] = new List<string>();

                        response.Errors[memberName].Add(validationResult.ErrorMessage);
                    }
                }

                response.Message = "Ошибка валидации данных";
                return response;
            }

            if (await _repository.ExistsByTitleAsync(request.Title))
            {
                response.Errors["Title"] = new List<string> { "Сериал с таким названием уже существует" };
                response.Message = "Сериал с таким названием уже существует";
                return response;
            }

            if (request.Genres == null || request.Genres.Count == 0)
            {
                response.Errors["Genres"] = new List<string> { "Укажите хотя бы один жанр" };
            }

            if (request.Cast == null || request.Cast.Count < 2)
            {
                response.Errors["Cast"] = new List<string> { "Укажите хотя бы двух актеров" };
            }

            if (response.Errors.Any())
            {
                response.Message = "Ошибка валидации данных";
                return response;
            }

            var seriesId = await _repository.AddSeriesAsync(request);

            response.Success = true;
            response.Message = "Сериал успешно добавлен";
            response.SeriesId = seriesId;

            return response;
        }
        catch (Exception ex)
        {
            response.Message = "Внутренняя ошибка сервера";
            return response;
        }
    }
}
