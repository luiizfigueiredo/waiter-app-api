using System.Collections.Generic;
using System.Threading.Tasks;
using WaiterApp.Models;
using WaiterApp.Repositories.Interfaces;
using WaiterApp.Services.Interfaces;

namespace WaiterApp.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;

    public CategoryService(
        ICategoryRepository categoryRepository,
        IProductRepository productRepository)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _categoryRepository.GetAllAsync();
    }

    public async Task<Category> CreateAsync(string name, string icon)
    {
        
        if (await _categoryRepository.CategoryAlreadyExists(name))
        {
            throw new InvalidOperationException($"Category with name '{name}' already exists.");
        }

        var category = new Category
        {
            Name = name,
            
            Icon = icon
        };

        return await _categoryRepository.CreateAsync(category);
    }

    public async Task<(List<Product> Products, Category? Category)> GetProductsByCategoryAsync(Guid categoryId)
    {
        var products = await _productRepository.GetByCategoryAsync(categoryId);
        var category = await _categoryRepository.GetByIdAsync(categoryId);

        return (products, category);
    }

}
