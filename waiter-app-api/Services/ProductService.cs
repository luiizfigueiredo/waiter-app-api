using System.Text.Json;
using WaiterApp.DTOs;
using WaiterApp.Models;
using WaiterApp.Repositories.Interfaces;
using WaiterApp.Services.Interfaces;

namespace WaiterApp.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product> CreateAsync(
        string name,
        string description,
        decimal price,
        string imagePath,
        Guid categoryId,
        string? ingredientsJson)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category is null)
            throw new InvalidOperationException($"Category with id '{categoryId}' not found.");

        var ingredients = new List<Ingredient>();
        if (!string.IsNullOrWhiteSpace(ingredientsJson))
        {
            var dtos = JsonSerializer.Deserialize<List<IngredientDto>>(
                ingredientsJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (dtos is not null)
            {
                ingredients = dtos.Select(d => new Ingredient
                {
                    Name = d.Name,
                    Icon = d.Icon
                }).ToList();
            }
        }

        var product = new Product
        {
            Name = name,
            Description = description,
            ImagePath = imagePath,
            Price = price,
            CategoryId = categoryId,
            Ingredients = ingredients
        };

        return await _productRepository.CreateAsync(product);
    }
}
