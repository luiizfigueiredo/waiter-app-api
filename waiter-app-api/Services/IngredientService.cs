using WaiterApp.Models;
using WaiterApp.Repositories.Interfaces;
using WaiterApp.Services.Interfaces;

namespace WaiterApp.Services;

public class IngredientService : IIngredientService
{
    private readonly IIngredientRepository _ingredientRepository;

    public IngredientService(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }

    public async Task<List<Ingredient>> GetAllAsync()
        => await _ingredientRepository.GetAllAsync();

    public async Task<Ingredient?> GetByIdAsync(Guid id)
        => await _ingredientRepository.GetByIdAsync(id);

    public async Task<Ingredient> CreateAsync(string name, string icon)
    {
        if (await _ingredientRepository.IngredientAlreadyExists(name))
            throw new InvalidOperationException($"Ingredient with name '{name}' already exists.");

        var ingredient = new Ingredient
        {
            Name = name,
            Icon = icon
        };

        return await _ingredientRepository.CreateAsync(ingredient);
    }

    public async Task<Ingredient> UpdateAsync(Guid id, string name, string icon)
    {
        var ingredient = await _ingredientRepository.GetByIdAsync(id);
        if (ingredient is null)
            throw new InvalidOperationException($"Ingredient with id '{id}' not found.");

        if (!string.Equals(ingredient.Name, name, StringComparison.OrdinalIgnoreCase)
            && await _ingredientRepository.IngredientAlreadyExists(name))
            throw new InvalidOperationException($"Ingredient with name '{name}' already exists.");

        ingredient.Name = name;
        ingredient.Icon = icon;

        return await _ingredientRepository.UpdateAsync(ingredient);
    }

    public async Task DeleteAsync(Guid id)
    {
        var ingredient = await _ingredientRepository.GetByIdAsync(id);
        if (ingredient is null)
            throw new InvalidOperationException($"Ingredient with id '{id}' not found.");

        await _ingredientRepository.DeleteAsync(ingredient);
    }
}
