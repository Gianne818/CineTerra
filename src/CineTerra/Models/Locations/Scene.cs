namespace CineTerra.Models.Locations;

/// <summary>
/// A verified film scene matched against its real-world counterpart.
/// </summary>
public sealed record Scene(
    string ProductionId,
    string Title,
    TimeSpan Timestamp,
    string StillImageUrl,
    string RealImageUrl,
    string SceneDescription)
{
    public string TimestampLabel => Timestamp.ToString(@"h\:mm\:ss");
}
