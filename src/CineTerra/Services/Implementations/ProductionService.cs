using CineTerra.Models.Productions;
using CineTerra.Services.Interfaces;

namespace CineTerra.Services.Implementations;

public sealed class ProductionService(HttpClient http)
    : JsonDataService<Production>(http, "data/productions.json"), IProductionService
{
    public Task<IReadOnlyList<Production>> GetAllAsync() => LoadAsync();

    public async Task<IReadOnlyList<T>> GetByTypeAsync<T>() where T : Production
    {
        var all = await LoadAsync();
        return all.OfType<T>().ToList();
    }
}
