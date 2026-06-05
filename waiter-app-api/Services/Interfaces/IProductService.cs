using WaiterApp.Models;

namespace WaiterApp.Services.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetAllAsync();
    Task<Product> CreateAsync(
        string name,
        string description,
        decimal price,
        string imagePath,
        Guid categoryId,
        string? ingredientsJson);
}
