namespace CineTerra.Models.Common;

/// <summary>
/// Immutable GPS coordinates value object.
/// </summary>
public sealed record Coordinates(double Latitude, double Longitude)
{
    public override string ToString() =>
        $"{Math.Abs(Latitude):F4}°{(Latitude >= 0 ? "N" : "S")}, " +
        $"{Math.Abs(Longitude):F4}°{(Longitude >= 0 ? "E" : "W")}";
}
