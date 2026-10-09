
using HotelSearch.Application.Dto;
using HotelSearch.Domain;

namespace HotelSearch.Application.Interfaces;

public interface IHotelService
{
    Task<IReadOnlyList<Hotel>> GetAllAsync(
        CancellationToken cancellationToken);

    Task<Hotel?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task<Hotel> CreateAsync(
        CreateHotelRequestDto request,
        CancellationToken cancellationToken);

    Task<bool> UpdateAsync(
        int id,
        CreateHotelRequestDto request,
        CancellationToken cancellationToken);

    Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken);

    Task<HotelSearchResponse> SearchAsync(
        string prompt,
        int page,
        int pageSize,
        CancellationToken cancellationToken);
}