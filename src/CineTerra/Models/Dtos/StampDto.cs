using CineTerra.Models.Passport;

namespace CineTerra.Models.Dtos;

public sealed record StampDto(
    string LocationId,
    string LocationName,
    string Country,
    DateTime DateCollected,
    string BadgeSlug)
{
    public static StampDto FromDomain(Stamp stamp) =>
        new(stamp.LocationId, stamp.LocationName, stamp.Country, stamp.DateCollected, stamp.BadgeSlug);

    public Stamp ToDomain() =>
        new(LocationId, LocationName, Country, DateCollected, BadgeSlug);
}
