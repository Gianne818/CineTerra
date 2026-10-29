using CineTerra.Models.Filters;
using CineTerra.Models.Locations;
using CineTerra.Services.Interfaces;

namespace CineTerra.Services.Implementations;

public sealed class LocationService(HttpClient http)
    : JsonDataService<FilmingLocation>(http, "data/locations.json"), ILocationService
{
    public Task<IReadOnlyList<FilmingLocation>> GetAllAsync() => LoadAsync();

    public async Task<IReadOnlyList<FilmingLocation>> GetByProductionAsync(string productionId)
    {
        var all = await LoadAsync();
        return all.Where(l => l.Scenes.Any(s => s.ProductionId.Equals(productionId, StringComparison.OrdinalIgnoreCase)))
                  .ToList();
    }

    public async Task<IReadOnlyList<FilmingLocation>> SearchAsync(LocationFilter filter)
    {
        var all = await LoadAsync();
        return Filter(all, filter);
    }

    public IReadOnlyList<FilmingLocation> Filter(IEnumerable<FilmingLocation> locations, LocationFilter filter)
    {
        ArgumentNullException.ThrowIfNull(locations);
        ArgumentNullException.ThrowIfNull(filter);

        return locations.Where(l => Matches(l, filter)).ToList();
    }

    public async Task<IReadOnlyList<FilmingLocation>> GetFeaturedAsync(int count = 6)
    {
        var all = await LoadAsync();
        return all.Take(count).ToList();
    }

    private static bool Matches(FilmingLocation location, LocationFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.SearchText))
        {
            var query = filter.SearchText.Trim();
            var matchesText =
                location.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                location.City.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                location.Country.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                location.Description.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                location.Scenes.Any(s => s.Title.Contains(query, StringComparison.OrdinalIgnoreCase));

            if (!matchesText) return false;
        }

        if (filter.SelectedRegion.HasValue && location.Region != filter.SelectedRegion.Value)
            return false;

        if (filter.SelectedDifficulty.HasValue && location.Accessibility != filter.SelectedDifficulty.Value)
            return false;

        return true;
    }
}
