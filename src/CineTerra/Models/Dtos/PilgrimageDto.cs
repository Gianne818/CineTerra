using CineTerra.Models.Pilgrimages;

namespace CineTerra.Models.Dtos;

public sealed record PilgrimageStopDto(
    string LocationId,
    string LocationName,
    string DisplayPlace,
    string ImageUrl,
    int Days);

public sealed record PilgrimageDto(string Name, List<PilgrimageStopDto> Stops)
{
    public static PilgrimageDto FromDomain(Pilgrimage pilgrimage) =>
        new(
            pilgrimage.Name,
            pilgrimage.Stops
                .Select(s => new PilgrimageStopDto(s.LocationId, s.LocationName, s.DisplayPlace, s.ImageUrl, s.Days))
                .ToList());

    public Pilgrimage ToDomain()
    {
        var domain = new Pilgrimage();
        var domainStops = Stops.Select(s =>
            new PilgrimageStop(s.LocationId, s.LocationName, s.DisplayPlace, s.ImageUrl, s.Days));
        domain.LoadFrom(Name, domainStops);
        return domain;
    }
}
