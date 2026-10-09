
using HotelSearch.Application.Interfaces;
using HotelSearch.Application.Search;
using HotelSearch.Infrastructure.Persistence;
using HotelSearch.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' was not found.");

builder.Services.AddDbContext<HotelSearchDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddSingleton<SearchCriteriaParser>();
builder.Services.AddScoped<IHotelService, HotelService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();