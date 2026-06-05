using Microsoft.EntityFrameworkCore;
using WaiterApp.Data;
using WaiterApp.Models;
using WaiterApp.Repositories.Interfaces;

namespace WaiterApp.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync()
        => await _context.Products
            .AsNoTracking()
            .Include(p => p.Ingredients)
            .Include(p => p.Category)
            .ToListAsync();

    public async Task<List<Product>> GetByCategoryAsync(Guid categoryId)
        => await _context.Products
            .AsNoTracking()
            .Include(p => p.Ingredients)
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();

    public async Task<List<Product>> GetByIdsAsync(IEnumerable<Guid> ids)
        => await _context.Products
            .AsNoTracking()
            .Include(p => p.Ingredients)
            .Include(p => p.Category)
            .Where(p => ids.Contains(p.Id))
            .ToListAsync();

    public async Task<Product?> GetByIdAsync(Guid id)
        => await _context.Products
            .AsNoTracking()
            .Include(p => p.Ingredients)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }
}
