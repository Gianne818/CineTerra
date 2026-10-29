using CineTerra.Models.Common;
using CineTerra.Models.Locations;

namespace CineTerra.Models.Pilgrimages;

public sealed class Pilgrimage : BaseEntity
{
    public const int MaxStops = 15;

    private readonly List<PilgrimageStop> _stops = [];

    public string Name { get; private set; } = "My Cinematic Pilgrimage";
    public IReadOnlyList<PilgrimageStop> Stops => _stops.AsReadOnly();
    public int TotalDays => _stops.Sum(s => s.Days);
    public int UniqueDestinationsCount => _stops.Select(s => s.DisplayPlace).Distinct().Count();
    public bool IsFull => _stops.Count >= MaxStops;
    public bool HasStops => _stops.Count > 0;

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return;
        Name = name.Trim();
    }

    public bool ContainsLocation(string locationId) =>
        _stops.Any(s => s.LocationId == locationId);

    public bool AddStop(FilmingLocation location, int days = 1)
    {
        ArgumentNullException.ThrowIfNull(location);
        if (IsFull || ContainsLocation(location.Id)) return false;

        _stops.Add(new PilgrimageStop(
            location.Id,
            location.Name,
            location.DisplayPlace,
            location.ImageUrl,
            Math.Max(1, days)));
        return true;
    }

    public bool RemoveStop(string locationId) =>
        _stops.RemoveAll(s => s.LocationId == locationId) > 0;

    public void UpdateDays(string locationId, int days)
    {
        var index = _stops.FindIndex(s => s.LocationId == locationId);
        if (index >= 0)
        {
            var existing = _stops[index];
            _stops[index] = existing with { Days = Math.Clamp(days, 1, 30) };
        }
    }

    public void MoveStop(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || fromIndex >= _stops.Count) return;
        if (toIndex < 0 || toIndex >= _stops.Count) return;
        if (fromIndex == toIndex) return;

        var stop = _stops[fromIndex];
        _stops.RemoveAt(fromIndex);
        _stops.Insert(toIndex, stop);
    }

    public void Clear() => _stops.Clear();

    public void LoadFrom(string name, IEnumerable<PilgrimageStop> stops)
    {
        Rename(name);
        _stops.Clear();
        _stops.AddRange(stops.Take(MaxStops));
    }
}
