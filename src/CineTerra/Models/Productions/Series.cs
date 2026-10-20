using CineTerra.Models.Enums;

namespace CineTerra.Models.Productions;

public sealed class Series : Production
{
    public int Seasons { get; init; }

    public override ProductionType Type => ProductionType.Series;

    public override string GetDurationLabel() =>
        Seasons == 1 ? "1 Season" : $"{Seasons} Seasons";
}
