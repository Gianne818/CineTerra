using CineTerra.Models.Enums;
using CineTerra.Models.Locations;

namespace CineTerra.Models.Filters;

public sealed class LocationFilter
{
    public string SearchText { get; set; } = string.Empty;
    public Genre? SelectedGenre { get; set; }
    public Region? SelectedRegion { get; set; }
    public Difficulty? SelectedDifficulty { get; set; }

    public bool IsActive =>
        !string.IsNullOrWhiteSpace(SearchText) ||
        SelectedGenre.HasValue ||
        SelectedRegion.HasValue ||
        SelectedDifficulty.HasValue;

    public void Clear()
    {
        SearchText = string.Empty;
        SelectedGenre = null;
        SelectedRegion = null;
        SelectedDifficulty = null;
    }
}
