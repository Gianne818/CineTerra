namespace CineTerra.Models.Pilgrimages;

public sealed record PilgrimageStop(
    string LocationId,
    string LocationName,
    string DisplayPlace,
    string ImageUrl,
    int Days = 1);
