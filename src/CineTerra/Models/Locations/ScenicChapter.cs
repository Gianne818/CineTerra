namespace CineTerra.Models.Locations;

/// <summary>
/// A curated scenic showcase chapter featured on the cinematic immersion timeline.
/// </summary>
public sealed record ScenicChapter(
    string LocationId,
    string ChapterTag,
    string FilmTitle,
    string LocationName,
    string ShortPlace,
    string FullPlace,
    string Narrative,
    string ImageUrl,
    string Alignment
);
