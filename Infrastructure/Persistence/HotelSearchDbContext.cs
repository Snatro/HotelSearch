using HotelSearch.Domain;
using Microsoft.EntityFrameworkCore;

namespace HotelSearch.Infrastructure.Persistence;

public class HotelSearchDbContext : DbContext
{
    public HotelSearchDbContext(
        DbContextOptions<HotelSearchDbContext> options)
        : base(options)
    {
    }

    public DbSet<Hotel> Hotels => Set<Hotel>();
}