using WaiterApp.Models;

namespace WaiterApp.Repositories.Interfaces;

public interface IIngredientRepository
{
    Task<List<Ingredient>> GetAllAsync();
    Task<Ingredient?> GetByIdAsync(Guid id);
    Task<List<Ingredient>> GetByIdsAsync(IEnumerable<Guid> ids);
    Task<Ingredient> CreateAsync(Ingredient ingredient);
    Task<Ingredient> UpdateAsync(Ingredient ingredient);
    Task DeleteAsync(Ingredient ingredient);

    Task<bool> IngredientAlreadyExists(string name);
}
