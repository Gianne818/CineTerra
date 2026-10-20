using CineTerra.Models.Passport;

namespace CineTerra.Services.Interfaces;

public interface IPassportService
{
    event Action? OnPassportChanged;
    Task InitializeAsync();
    Task<IReadOnlyList<Stamp>> GetCollectedStampsAsync();
    Task<bool> HasStampAsync(string locationId);
    Task<bool> CollectStampAsync(string locationId, string locationName, string country, string badgeSlug);
    Task ResetStampsAsync();
}
