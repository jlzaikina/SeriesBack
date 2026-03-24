using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Ticket_vkr.UI.Application.Handlers;
using Ticket_vkr.UI.Domain.Status;

namespace Ticket_vkr.UI.Adapters.Http;

public class Controller : ControllerBase
{
    private readonly IHandler _handler;

    public Controller(IHandler handler)
    {
        _handler = handler ?? throw new ArgumentNullException(nameof(handler));
    }

    [HttpGet("getstatuses")]
    public async Task<IActionResult> GetStatuses(string deviceId)
    {
        var statuses = await _handler.GetUserStatusesAsync(deviceId);
        return Ok(statuses);
    }

    [HttpPost("updatestatus")]
    public async Task<IActionResult> UpdateStatus([FromForm] UpdateStatusRequest request)
    {
        var result = await _handler.UpdateStatusAsync(request);
        return Ok(result);
    }

    [HttpGet("getnews")]
    public async Task<IActionResult> GetNews(string sortBy = null, bool ascending = true)
    {
        var news = await _handler.GetNewsAsync(sortBy, ascending);
        return Ok(news);
    }

    [HttpGet("getseries")]
    public async Task<IActionResult> GetSeries(int page = 1, int pageSize = 6)
    {
        var result = await _handler.GetPagedSeriesAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("filter/by-year")]
    public async Task<IActionResult> FilterByYear([FromQuery] int from, [FromQuery] int to)
    {
        var result = await _handler.FilterByYearAsync(from, to);
        return Ok(result);
    }

    [HttpGet("filter/by-genre")]
    public async Task<IActionResult> FilterByGenre([FromQuery] string genre)
    {
        var result = await _handler.FilterByGenreAsync(genre);
        return Ok(result);
    }

    [HttpGet("filter/combined")]
    public async Task<IActionResult> FilterCombined(
        [FromQuery] int from,
        [FromQuery] int to,
        [FromQuery] string genre)
    {
        var result = await _handler.FilterByYearAndGenreAsync(from, to, genre);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddSeries([FromBody] AddSeriesRequest request)
    {

        var response = await _handler.AddSeriesAsync(request);

        if (response.Success)
            return Ok(response);
        else
            return BadRequest(response);
    }
}