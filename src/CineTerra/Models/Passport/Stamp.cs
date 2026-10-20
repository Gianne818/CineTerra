namespace CineTerra.Models.Passport;

/// <summary>
/// A collectible ink stamp earned by visiting or exploring a cinema coordinate.
/// </summary>
public sealed record Stamp(
    string LocationId,
    string LocationName,
    string Country,
    DateTime DateCollected,
    string BadgeSlug);
