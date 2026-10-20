using System.Net.Http.Json;
using CineTerra.Models.Common;
using CineTerra.Utilities;

namespace CineTerra.Services.Implementations;

/// <summary>
/// Abstract generic service handling JSON file fetching, deserialization, and session caching.
/// </summary>
public abstract class JsonDataService<T> where T : BaseEntity
{
    private readonly HttpClient _http;
    private readonly string _dataPath;
    private IReadOnlyList<T>? _cache;

    protected JsonDataService(HttpClient http, string dataPath)
    {
        _http = http;
        _dataPath = dataPath;
    }

    protected async Task<IReadOnlyList<T>> LoadAsync()
    {
        if (_cache is not null) return _cache;

        try
        {
            var data = await _http.GetFromJsonAsync<List<T>>(_dataPath, JsonDefaults.Options);
            _cache = data ?? [];
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error loading {_dataPath}: {ex.Message}");
            _cache = [];
        }

        return _cache;
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        var list = await LoadAsync();
        return list.FirstOrDefault(e => e.Id == id);
    }
}
