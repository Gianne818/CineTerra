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
        return all.Where(filter.Matches).ToList();
    }

    public async Task<IReadOnlyList<FilmingLocation>> GetFeaturedAsync(int count = 6)
    {
        var all = await LoadAsync();
        return all.Take(count).ToList();
    }
}
