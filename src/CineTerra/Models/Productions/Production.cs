using System.Text.Json.Serialization;
using CineTerra.Models.Common;
using CineTerra.Models.Enums;

namespace CineTerra.Models.Productions;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(Film), "film")]
[JsonDerivedType(typeof(Series), "series")]
[JsonDerivedType(typeof(Anime), "anime")]
public abstract class Production : BaseEntity
{
    public string Title { get; init; } = string.Empty;
    public int ReleaseYear { get; init; }
    public string Director { get; init; } = string.Empty;
    public string Synopsis { get; init; } = string.Empty;
    public string PosterUrl { get; init; } = string.Empty;
    public string BackdropUrl { get; init; } = string.Empty;
    public IReadOnlyList<Genre> Genres { get; init; } = [];

    [JsonIgnore]
    public abstract ProductionType Type { get; }

    /// <summary>
    /// Polymorphic duration label formatting.
    /// </summary>
    public abstract string GetDurationLabel();

    public virtual string GetDisplayTitle() => $"{Title} ({ReleaseYear})";
}
