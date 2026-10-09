
using System.Globalization;
using System.Text.RegularExpressions;

namespace HotelSearch.Application.Search;

public sealed record SearchCriteria(
    string Location,
    double Latitude,
    double Longitude,
    decimal Budget);

public sealed class SearchCriteriaParser
{
    private static readonly Dictionary<string, (double Lat, double Lon)> Cities =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["Zagreb"] = (45.8150, 15.9819),
            ["Split"] = (43.5081, 16.4402),
            ["Zadar"] = (44.1194, 15.2314),
            ["Rijeka"] = (45.3271, 14.4422),
            ["Dubrovnik"] = (42.6507, 18.0944)
        };

    public SearchCriteria Parse(string prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt))
            throw new ArgumentException("Prompt is required.");

        var cityMatch = Regex.Match(
            prompt,
            @"\b(?:in|near|around|u|blizu|oko)\s+([a-zA-ZčćžšđČĆŽŠĐ]+)",
            RegexOptions.IgnoreCase);

        if (!cityMatch.Success)
            throw new ArgumentException(
                "Could not identify a city in the prompt.");

        var city = cityMatch.Groups[1].Value;

        if (!Cities.TryGetValue(city, out var coordinates))
            throw new ArgumentException(
                $"Unsupported city: {city}.");

        var budgetMatch = Regex.Match(
            prompt,
            @"(?:under|below|budget(?:\s+of)?|do|max(?:imum)?|ispod|maksimalno)"
            + @"\s*€?\s*(\d+(?:[.,]\d{1,2})?)"
            + @"\s*(?:€|eur(?:os?)?)?",
            RegexOptions.IgnoreCase);

        if (!budgetMatch.Success)
            throw new ArgumentException(
                "Could not identify a budget in the prompt.");

        var budgetText = budgetMatch.Groups[1].Value
            .Replace(',', '.');

        if (!decimal.TryParse(
                budgetText,
                NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out var budget)
            || budget <= 0)
        {
            throw new ArgumentException(
                "Budget must be a positive number.");
        }

        return new SearchCriteria(
            city,
            coordinates.Lat,
            coordinates.Lon,
            budget);
    }
}