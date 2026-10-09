
using HotelSearch.Application.Dto;
using HotelSearch.Application.Interfaces;
using HotelSearch.Application.Search;
using HotelSearch.Domain;
using HotelSearch.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace HotelSearch.Infrastructure.Services;



public sealed class HotelService : IHotelService
{
    private const double EarthRadiusKm = 6371.0;

    private readonly HotelSearchDbContext _dbContext;
    private readonly SearchCriteriaParser _parser;

    public HotelService(
        HotelSearchDbContext dbContext,
        SearchCriteriaParser parser)
    {
        _dbContext = dbContext;
        _parser = parser;
    }
    public async Task<IReadOnlyList<Hotel>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await _dbContext.Hotels
            .AsNoTracking()
            .OrderBy(h => h.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Hotel?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Hotels
            .AsNoTracking()
            .FirstOrDefaultAsync(
                h => h.Id == id,
                cancellationToken);
    }

    public async Task<Hotel> CreateAsync(
     CreateHotelRequestDto request,
     CancellationToken cancellationToken)
    {
        var hotel = new Hotel
        {
            Name = request.Name.Trim(),
            Price = request.Price,
            Latitude = request.Latitude,
            Longitude = request.Longitude
        };

        _dbContext.Hotels.Add(hotel);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return hotel;
    }

    public async Task<bool> UpdateAsync(
     int id,
     CreateHotelRequestDto request,
     CancellationToken cancellationToken)
    {
        var hotel = await _dbContext.Hotels
            .FirstOrDefaultAsync(
                h => h.Id == id,
                cancellationToken);

        if (hotel is null)
            return false;

        hotel.Name = request.Name.Trim();
        hotel.Price = request.Price;
        hotel.Latitude = request.Latitude;
        hotel.Longitude = request.Longitude;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var hotel = await _dbContext.Hotels
            .FirstOrDefaultAsync(
                h => h.Id == id,
                cancellationToken);

        if (hotel is null)
        {
            return false;
        }

        _dbContext.Hotels.Remove(hotel);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<HotelSearchResponse> SearchAsync(
        string prompt,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (page < 1)
            throw new ArgumentOutOfRangeException(nameof(page));

        if (pageSize < 1 || pageSize > 100)
            throw new ArgumentOutOfRangeException(nameof(pageSize));

        var criteria = _parser.Parse(prompt);

        var originLat = criteria.Latitude * Math.PI / 180.0;
        var originLon = criteria.Longitude * Math.PI / 180.0;

        var hotels = _dbContext.Hotels
            .AsNoTracking()
            .Where(h => h.Price <= criteria.Budget);

        var candidates = hotels.Select(h => new
        {
            Hotel = h,

            DistanceKm = EarthRadiusKm * 2.0 * Math.Asin(
                Math.Sqrt(
                    Math.Pow(
                        Math.Sin(
                            (h.Latitude * Math.PI / 180.0
                                - originLat) / 2.0),
                        2.0)
                    +
                    Math.Cos(originLat)
                    * Math.Cos(h.Latitude * Math.PI / 180.0)
                    * Math.Pow(
                        Math.Sin(
                            (h.Longitude * Math.PI / 180.0
                                - originLon) / 2.0),
                        2.0)
                ))
        });

        var ranked = candidates.Select(x => new
        {
            x.Hotel,
            x.DistanceKm,

            Score =
                0.5 * (double)(x.Hotel.Price / criteria.Budget)
                + 0.5 * (
                    x.DistanceKm / (x.DistanceKm + 100.0))
        });

        var totalCount = await ranked.CountAsync(cancellationToken);

        var results = await ranked
          .OrderBy(x => x.Score)
          .ThenBy(x => x.Hotel.Id)
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .Select(x => new HotelSearchResultDto(
              x.Hotel.Id,
              x.Hotel.Name,
              x.Hotel.Price,
              x.DistanceKm))
          .ToListAsync(cancellationToken);

        return new HotelSearchResponse(
            results,
            page,
            pageSize,
            totalCount);
    }
}