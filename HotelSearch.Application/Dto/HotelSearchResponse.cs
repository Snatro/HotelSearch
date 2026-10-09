using HotelSearch.Application.Dto;

public sealed record HotelSearchResponse(
    IReadOnlyList<HotelSearchResultDto> Hotels,
    int Page,
    int PageSize,
    int TotalCount);