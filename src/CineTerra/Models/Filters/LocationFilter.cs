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

    public bool Matches(FilmingLocation location)
    {
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var query = SearchText.Trim();
            var matchesText =
                location.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                location.City.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                location.Country.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                location.Description.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                location.Scenes.Any(s => s.Title.Contains(query, StringComparison.OrdinalIgnoreCase));

            if (!matchesText) return false;
        }

        if (SelectedRegion.HasValue && location.Region != SelectedRegion.Value)
            return false;

        if (SelectedDifficulty.HasValue && location.Accessibility != SelectedDifficulty.Value)
            return false;

        return true;
    }
}
