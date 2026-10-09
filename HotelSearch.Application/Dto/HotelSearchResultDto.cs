namespace HotelSearch.Application.Dto;

public sealed record HotelSearchResultDto(
    int Id,
    string Name,
    decimal Price,
    double DistanceKm);