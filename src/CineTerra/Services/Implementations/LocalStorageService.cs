using System.Text.Json;
using Microsoft.JSInterop;
using CineTerra.Services.Interfaces;
using CineTerra.Utilities;

namespace CineTerra.Services.Implementations;

public sealed class LocalStorageService(IJSRuntime js) : ILocalStorageService
{
    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var json = await js.InvokeAsync<string?>("localStorage.getItem", key);
            if (string.IsNullOrWhiteSpace(json)) return default;
            return JsonSerializer.Deserialize<T>(json, JsonDefaults.Options);
        }
        catch
        {
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value)
    {
        try
        {
            var json = JsonSerializer.Serialize(value, JsonDefaults.Options);
            await js.InvokeVoidAsync("localStorage.setItem", key, json);
        }
        catch
        {
            // Ignore in environments where localStorage is restricted
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            await js.InvokeVoidAsync("localStorage.removeItem", key);
        }
        catch
        {
            // Ignore
        }
    }
}
