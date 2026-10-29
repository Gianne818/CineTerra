using CineTerra.Models.Dtos;
using CineTerra.Models.Locations;
using CineTerra.Models.Pilgrimages;
using CineTerra.Services.Interfaces;
using CineTerra.Utilities;

namespace CineTerra.State;

public sealed class PilgrimageState(ILocalStorageService storage, ILocationService locationService)
{
    private bool _initialized;

    public Pilgrimage Current { get; private set; } = new();

    public event Action? OnChange;

    public async Task InitializeAsync()
    {
        if (_initialized) return;

        var dto = await storage.GetAsync<PilgrimageDto>(StorageKeys.Pilgrimage);
        if (dto is not null)
        {
            var locations = await locationService.GetAllAsync();
            var locationMap = locations.ToDictionary(l => l.Id, l => l.ImageUrl);
            var refreshedStops = dto.Stops
                .Select(s => locationMap.TryGetValue(s.LocationId, out var latestImg) && !string.IsNullOrWhiteSpace(latestImg)
                    ? s with { ImageUrl = latestImg }
                    : s)
                .ToList();
            Current = (dto with { Stops = refreshedStops }).ToDomain();
        }

        _initialized = true;
        NotifyStateChanged();
    }

    public async Task<bool> AddStopAsync(FilmingLocation location, int days = 1)
    {
        await InitializeAsync();
        var added = Current.AddStop(location, days);
        if (added)
        {
            await PersistAsync();
        }
        return added;
    }

    public async Task<bool> RemoveStopAsync(string locationId)
    {
        await InitializeAsync();
        var removed = Current.RemoveStop(locationId);
        if (removed)
        {
            await PersistAsync();
        }
        return removed;
    }

    public async Task UpdateDaysAsync(string locationId, int days)
    {
        await InitializeAsync();
        Current.UpdateDays(locationId, days);
        await PersistAsync();
    }

    public async Task MoveStopAsync(int fromIndex, int toIndex)
    {
        await InitializeAsync();
        Current.MoveStop(fromIndex, toIndex);
        await PersistAsync();
    }

    public async Task RenameAsync(string name)
    {
        await InitializeAsync();
        Current.Rename(name);
        await PersistAsync();
    }

    public async Task ClearAsync()
    {
        await InitializeAsync();
        Current.Clear();
        await storage.RemoveAsync(StorageKeys.Pilgrimage);
        NotifyStateChanged();
    }

    private async Task PersistAsync()
    {
        await storage.SetAsync(StorageKeys.Pilgrimage, PilgrimageDto.FromDomain(Current));
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
