using CineTerra.Models.Dtos;
using CineTerra.Models.Passport;
using CineTerra.Services.Interfaces;
using CineTerra.Utilities;

namespace CineTerra.Services.Implementations;

public sealed class PassportService(ILocalStorageService storage) : IPassportService
{
    private List<Stamp> _stamps = [];
    private bool _initialized;

    public event Action? OnPassportChanged;

    public async Task InitializeAsync()
    {
        if (_initialized) return;

        var dtos = await storage.GetAsync<List<StampDto>>(StorageKeys.Passport);
        _stamps = dtos?.Select(d => d.ToDomain()).ToList() ?? [];
        _initialized = true;
        OnPassportChanged?.Invoke();
    }

    public async Task<IReadOnlyList<Stamp>> GetCollectedStampsAsync()
    {
        await InitializeAsync();
        return _stamps.AsReadOnly();
    }

    public async Task<bool> HasStampAsync(string locationId)
    {
        await InitializeAsync();
        return _stamps.Any(s => s.LocationId == locationId);
    }

    public async Task<bool> CollectStampAsync(string locationId, string locationName, string country, string badgeSlug)
    {
        await InitializeAsync();

        if (_stamps.Any(s => s.LocationId == locationId))
            return false;

        var newStamp = new Stamp(locationId, locationName, country, DateTime.UtcNow, badgeSlug);
        _stamps.Add(newStamp);

        await storage.SetAsync(StorageKeys.Passport, _stamps.Select(StampDto.FromDomain).ToList());
        OnPassportChanged?.Invoke();
        return true;
    }

    public async Task ResetStampsAsync()
    {
        _stamps.Clear();
        await storage.RemoveAsync(StorageKeys.Passport);
        OnPassportChanged?.Invoke();
    }
}
