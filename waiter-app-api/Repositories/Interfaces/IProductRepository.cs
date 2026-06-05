using WaiterApp.Models;

namespace WaiterApp.Repositories.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<List<Product>> GetByCategoryAsync(Guid categoryId);
    Task<List<Product>> GetByIdsAsync(IEnumerable<Guid> ids);
    Task<Product?> GetByIdAsync(Guid id);
    Task<Product> CreateAsync(Product product);
}
