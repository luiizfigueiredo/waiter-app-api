using Microsoft.EntityFrameworkCore;
using WaiterApp.Data;
using WaiterApp.Models;
using WaiterApp.Repositories.Interfaces;

namespace WaiterApp.Repositories;

public class IngredientRepository : IIngredientRepository
{
    private readonly AppDbContext _context;

    public IngredientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IngredientAlreadyExists(string name)
        => await _context.Ingredients.AnyAsync(i => i.Name.ToLower() == name.ToLower());

    public async Task<List<Ingredient>> GetAllAsync()
        => await _context.Ingredients.AsNoTracking().ToListAsync();

    public async Task<Ingredient?> GetByIdAsync(Guid id)
        => await _context.Ingredients.FirstOrDefaultAsync(i => i.Id == id);

    public async Task<List<Ingredient>> GetByIdsAsync(IEnumerable<Guid> ids)
        => await _context.Ingredients.Where(i => ids.Contains(i.Id)).ToListAsync();

    public async Task<Ingredient> CreateAsync(Ingredient ingredient)
    {
        _context.Ingredients.Add(ingredient);
        await _context.SaveChangesAsync();
        return ingredient;
    }

    public async Task<Ingredient> UpdateAsync(Ingredient ingredient)
    {
        await _context.SaveChangesAsync();
        return ingredient;
    }

    public async Task DeleteAsync(Ingredient ingredient)
    {
        _context.Ingredients.Remove(ingredient);
        await _context.SaveChangesAsync();
    }
}
