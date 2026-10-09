
using HotelSearch.Application.Dto;
using HotelSearch.Application.Interfaces;
using HotelSearch.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
namespace HotelSearch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelsController : ControllerBase
{
    private readonly IHotelService _hotelService;

    public HotelsController(IHotelService hotelService)
    {
        _hotelService = hotelService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Hotel>>> GetAll(
        CancellationToken cancellationToken)
    {
        var hotels = await _hotelService.GetAllAsync(cancellationToken);

        return Ok(hotels);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Hotel>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var hotel = await _hotelService.GetByIdAsync(
            id, cancellationToken);

        if (hotel is null)
            return NotFound();

        return Ok(hotel);
    }

    [HttpPost]
    public async Task<ActionResult<Hotel>> Create(
        [FromBody] CreateHotelRequestDto request,
        CancellationToken cancellationToken)
    {
        var hotel = await _hotelService.CreateAsync(
            request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = hotel.Id },
            hotel);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] CreateHotelRequestDto request,
        CancellationToken cancellationToken)
    {
        var updated = await _hotelService.UpdateAsync(
            id, request, cancellationToken);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var deleted = await _hotelService.DeleteAsync(
            id, cancellationToken);

        if (!deleted)
            return NotFound();

        return NoContent();
    }


    [HttpPost("search")]
    public async Task<ActionResult<HotelSearchResponse>> Search(
        [FromBody] SearchHotelsRequest request,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Prompt))
        {
            return BadRequest(new
            {
                error = "Search prompt is required."
            });
        }

        if (page < 1 || pageSize < 1 || pageSize > 100)
        {
            return BadRequest(new
            {
                error = "Page must be positive and pageSize must be between 1 and 100."
            });
        }

        var results = await _hotelService.SearchAsync(
            request.Prompt.Trim(),
            page,
            pageSize,
            cancellationToken);

        return Ok(results);
    }
}