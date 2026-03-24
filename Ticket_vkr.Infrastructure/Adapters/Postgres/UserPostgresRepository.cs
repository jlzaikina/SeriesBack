using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using Ticket_vkr.UI.Domain.Status;
using Ticket_vkr.UI.Entities;
using Ticket_vkr.UI.Ports;

namespace Ticket_vkr.Infrastructure.Adapters.Postgres;


public class UserPostgresRepository : IRepository
{
    private readonly ApplicationDbContext _context;

    public UserPostgresRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserSeriesStatus>> GetByDeviceIdAsync(string deviceId)
    {
        return await _context.UserSeriesStatuses
            .AsNoTracking()
            .Where(s => s.UserDeviceId == deviceId)
            .ToListAsync();
    }

    public async Task<UserSeriesStatus> GetByDeviceAndSeriesAsync(string deviceId, string seriesId)
    {
        return await _context.UserSeriesStatuses
            .FirstOrDefaultAsync(s => s.UserDeviceId == deviceId && s.SeriesId == seriesId);
    }
    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<List<News>> GetAllAsync()
    {
        return await _context.News.ToListAsync();
    }

    public async Task<List<News>> GetSortedAsync(string sortBy, bool ascending)
    {
        var query = _context.News.AsQueryable();

        query = sortBy?.ToLower() switch
        {
            "title" => ascending ? query.OrderBy(n => n.Title) : query.OrderByDescending(n => n.Title),
            "platform" => ascending ? query.OrderBy(n => n.Platform) : query.OrderByDescending(n => n.Platform),
            "date" => ascending ? query.OrderBy(n => n.PublicationDate) : query.OrderByDescending(n => n.PublicationDate),
            "genre" => ascending ? query.OrderBy(n => n.Genre) : query.OrderByDescending(n => n.Genre),
            "status" => ascending ? query.OrderBy(n => n.Status) : query.OrderByDescending(n => n.Status),
            _ => query.OrderBy(n => n.PublicationDate)
        };

        return await query.ToListAsync();
    }

    public async Task<(List<Series> Items, int Total)> GetPagedAsync(int page, int pageSize)
    {
        var query = _context.Series
            .Include(s => s.SeriesGenres)
                .ThenInclude(sg => sg.Genre)
                .OrderBy(s => s.CreatedAt);

        var total = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }
    public async Task<List<Series>> FilterByYearAsync(int from, int to)
    {
        return await _context.Series
            .Include(s => s.SeriesGenres)
                .ThenInclude(sg => sg.Genre)
            .Where(s => s.ReleaseYearStart >= from && s.ReleaseYearStart <= to)
            .OrderBy(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Series>> FilterByGenreAsync(string genre)
    {
        return await _context.Series
            .Include(s => s.SeriesGenres)
                .ThenInclude(sg => sg.Genre)
            .Where(s => s.SeriesGenres.Any(sg => sg.Genre.Name == genre))
            .OrderBy(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Series>> FilterByYearAndGenreAsync(int from, int to, string genre)
    {
        return await _context.Series
            .Include(s => s.SeriesGenres)
                .ThenInclude(sg => sg.Genre)
            .Where(s => s.ReleaseYearStart >= from && s.ReleaseYearStart <= to)
            .Where(s => s.SeriesGenres.Any(sg => sg.Genre.Name == genre))
            .OrderBy(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> ExistsByTitleAsync(string title)
    {
        return await _context.Series.AnyAsync(s => s.Title == title);
    }

    // Получение или создание жанра
    public async Task<Genres> GetOrCreateGenreAsync(string genreName)
    {
        var genre = await _context.Genres
            .FirstOrDefaultAsync(g => g.Name == genreName);

        if (genre == null)
        {
            genre = new Genres { Name = genreName };
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
        }

        return genre;
    }

    // Получение или создание режиссера
    public async Task<Director> GetOrCreateDirectorAsync(string directorName)
    {
        var director = await _context.Directors
            .FirstOrDefaultAsync(d => d.Name == directorName);

        if (director == null)
        {
            director = new Director { Name = directorName };
            _context.Directors.Add(director);
            await _context.SaveChangesAsync();
        }

        return director;
    }

    // Получение или создание актера
    public async Task<Actor> GetOrCreateActorAsync(string actorName)
    {
        var actor = await _context.Actors
            .FirstOrDefaultAsync(a => a.Name == actorName);

        if (actor == null)
        {
            actor = new Actor { Name = actorName };
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();
        }

        return actor;
    }

    // Добавление сериала
    public async Task<Guid> AddSeriesAsync(AddSeriesRequest request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var seriesId = Guid.NewGuid();

            string posterPath = null;
            if (!string.IsNullOrEmpty(request.Poster))
            {
                var fileName = request.Poster.TrimStart('/');
                posterPath = $"/catalog/img/{fileName}";
            }

            var series = new Series
            {
                Id = seriesId,
                Title = request.Title,
                ReleaseYearStart = request.Year,
                SeasonsCount = request.Seasons,
                EpisodeLength = $"{request.Duration} мин",
                Rating = request.Rating,
                Country = request.Country,
                PosterPath = posterPath,
                Plot = request.Description,
                CreatedAt = DateTime.Now,
            };

            _context.Series.Add(series);

            var director = await GetOrCreateDirectorAsync(request.Director);
            _context.SeriesDirectors.Add(new SeriesDirector
            {
                SeriesId = seriesId,
                DirectorId = director.Id
            });

            foreach (var genreName in request.Genres.Where(g => !string.IsNullOrWhiteSpace(g)))
            {
                var genre = await GetOrCreateGenreAsync(genreName.Trim());
                _context.SeriesGenres.Add(new SeriesGenre
                {
                    SeriesId = seriesId,
                    GenreId = genre.Id
                });
            }

            foreach (var actorName in request.Cast.Where(a => !string.IsNullOrWhiteSpace(a)))
            {
                var actor = await GetOrCreateActorAsync(actorName.Trim());
                _context.SeriesActors.Add(new SeriesActor
                {
                    SeriesId = seriesId,
                    ActorId = actor.Id
                });
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return seriesId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
