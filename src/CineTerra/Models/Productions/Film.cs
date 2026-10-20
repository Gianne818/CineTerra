using CineTerra.Models.Enums;

namespace CineTerra.Models.Productions;

public sealed class Film : Production
{
    public int RuntimeMinutes { get; init; }

    public override ProductionType Type => ProductionType.Film;

    public override string GetDurationLabel() =>
        $"{RuntimeMinutes / 60}h {RuntimeMinutes % 60}m";
}
