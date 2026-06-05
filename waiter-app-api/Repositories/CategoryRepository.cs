using Microsoft.EntityFrameworkCore;
using WaiterApp.Data;
using WaiterApp.Models;
using WaiterApp.Repositories.Interfaces;

namespace WaiterApp.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CategoryAlreadyExists(string name) 
        => await _context.Categories.AnyAsync(c => c.Name.ToLower() == name.ToLower());


    public async Task<List<Category>> GetAllAsync()
        => await _context.Categories.AsNoTracking().ToListAsync();

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Category>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        return await _context.Categories
                .AsNoTracking()
                .Where(c => ids.Contains(c.Id))
                .ToListAsync();
    }

    public async Task<Category> CreateAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }
}
