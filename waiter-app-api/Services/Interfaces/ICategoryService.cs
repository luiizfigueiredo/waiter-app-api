using WaiterApp.Models;

namespace WaiterApp.Services.Interfaces;

public interface ICategoryService
{
    Task<List<Category>> GetAllAsync();
    Task<Category> CreateAsync(string name, string icon);
    Task<(List<Product> Products, Category? Category)> GetProductsByCategoryAsync(Guid categoryId);
}
