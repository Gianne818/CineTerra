using CineTerra.Models.Productions;

namespace CineTerra.Services.Interfaces;

public interface IProductionService
{
    Task<IReadOnlyList<Production>> GetAllAsync();
    Task<Production?> GetByIdAsync(string id);
    Task<IReadOnlyList<T>> GetByTypeAsync<T>() where T : Production;
}
