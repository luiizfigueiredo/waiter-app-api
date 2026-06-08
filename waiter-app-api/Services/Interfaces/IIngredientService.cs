using WaiterApp.Models;

namespace WaiterApp.Services.Interfaces;

public interface IIngredientService
{
    Task<List<Ingredient>> GetAllAsync();
    Task<Ingredient?> GetByIdAsync(Guid id);
    Task<Ingredient> CreateAsync(string name, string icon);
    Task<Ingredient> UpdateAsync(Guid id, string name, string icon);
    Task DeleteAsync(Guid id);
}
