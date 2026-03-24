using Ticket_vkr.UI.Domain.Status;
using Ticket_vkr.UI.Entities;

namespace Ticket_vkr.UI.Ports;

/// <summary>
/// Работа с пользователем в бд
/// </summary>

public interface IRepository
{
    Task<List<UserSeriesStatus>> GetByDeviceIdAsync(string deviceId);
    Task<UserSeriesStatus> GetByDeviceAndSeriesAsync(string deviceId, string seriesId);
    Task<bool> SaveChangesAsync();
    Task<List<News>> GetAllAsync();
    Task<List<News>> GetSortedAsync(string sortBy, bool ascending);
    Task<(List<Series> Items, int Total)> GetPagedAsync(int page, int pageSize);
    Task<List<Series>> FilterByYearAsync(int from, int to);
    Task<List<Series>> FilterByGenreAsync(string genre);
    Task<List<Series>> FilterByYearAndGenreAsync(int from, int to, string genre);
    Task<Guid> AddSeriesAsync(AddSeriesRequest request);
    Task<Actor> GetOrCreateActorAsync(string actorName);
    Task<Director> GetOrCreateDirectorAsync(string directorName);
    Task<Genres> GetOrCreateGenreAsync(string genreName);
    Task<bool> ExistsByTitleAsync(string title);
}