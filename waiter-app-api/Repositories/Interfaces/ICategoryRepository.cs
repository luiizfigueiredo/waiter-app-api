using WaiterApp.Models;

namespace WaiterApp.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(Guid id);
    Task<List<Category>> GetByIdsAsync(IEnumerable<Guid> ids);
    Task<Category> CreateAsync(Category category);

    Task<bool> CategoryAlreadyExists(string name);
}
