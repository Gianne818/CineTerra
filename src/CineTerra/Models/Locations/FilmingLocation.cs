using CineTerra.Models.Common;
using CineTerra.Models.Enums;

namespace CineTerra.Models.Locations;

public sealed class FilmingLocation : BaseEntity
{
    public string Name { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public Region Region { get; init; }
    public Coordinates Coordinates { get; init; } = new(0, 0);
    public string Description { get; init; } = string.Empty;
    public string PracticalTips { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
    public Difficulty Accessibility { get; init; }
    public IReadOnlyList<Scene> Scenes { get; init; } = [];

    public bool HasScenes => Scenes.Count > 0;
    public string DisplayPlace => string.IsNullOrWhiteSpace(City) ? Country : $"{City}, {Country}";
}
