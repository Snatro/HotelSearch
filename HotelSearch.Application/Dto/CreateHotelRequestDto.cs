namespace HotelSearch.Application.Dto;

public sealed record CreateHotelRequestDto(
    string Name,
    decimal Price,
    double Latitude,
    double Longitude);