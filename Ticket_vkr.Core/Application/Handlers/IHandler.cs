using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket_vkr.UI.Domain.Status;
using Ticket_vkr.UI.Entities;

namespace Ticket_vkr.UI.Application.Handlers;

public interface IHandler
{
    Task<Dictionary<string, string>> GetUserStatusesAsync(string deviceId);
    Task<bool> UpdateStatusAsync(UpdateStatusRequest request);
    Task<List<News>> GetNewsAsync(string sortBy = null, bool ascending = true);
    Task<object> GetPagedSeriesAsync(int page, int pageSize);
    Task<object> FilterByYearAsync(int from, int to);
    Task<object> FilterByGenreAsync(string genre);
    Task<object> FilterByYearAndGenreAsync(int from, int to, string genre);
    Task<AddSeriesResponse> AddSeriesAsync(AddSeriesRequest request);
}
