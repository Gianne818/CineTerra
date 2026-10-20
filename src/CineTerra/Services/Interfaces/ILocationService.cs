using CineTerra.Models.Filters;
using CineTerra.Models.Locations;

namespace CineTerra.Services.Interfaces;

public interface ILocationService
{
    Task<IReadOnlyList<FilmingLocation>> GetAllAsync();
    Task<FilmingLocation?> GetByIdAsync(string id);
    Task<IReadOnlyList<FilmingLocation>> GetByProductionAsync(string productionId);
    Task<IReadOnlyList<FilmingLocation>> SearchAsync(LocationFilter filter);
    Task<IReadOnlyList<FilmingLocation>> GetFeaturedAsync(int count = 6);
}
