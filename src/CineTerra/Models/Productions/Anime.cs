using CineTerra.Models.Enums;

namespace CineTerra.Models.Productions;

public sealed class Anime : Production
{
    public string Studio { get; init; } = string.Empty;
    public int Episodes { get; init; } // 0 = feature film

    public override ProductionType Type => ProductionType.Anime;

    public override string GetDurationLabel() =>
        Episodes == 0 ? "Feature Film" : $"{Episodes} Episodes";

    public override string GetDisplayTitle() =>
        string.IsNullOrEmpty(Studio) ? base.GetDisplayTitle() : $"{base.GetDisplayTitle()} · {Studio}";
}
