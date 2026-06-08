using WaiterApp.Models;
using WaiterApp.Repositories.Interfaces;
using WaiterApp.Services.Interfaces;

namespace WaiterApp.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IIngredientRepository _ingredientRepository;

    public ProductService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IIngredientRepository ingredientRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _ingredientRepository = ingredientRepository;
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
        List<Guid>? ingredientIds)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category is null)
            throw new InvalidOperationException($"Category with id '{categoryId}' not found.");

        var ingredients = new List<Ingredient>();
        if (ingredientIds is { Count: > 0 })
        {
            ingredients = await _ingredientRepository.GetByIdsAsync(ingredientIds);

            var missingIds = ingredientIds.Except(ingredients.Select(i => i.Id)).ToList();
            if (missingIds.Count > 0)
                throw new InvalidOperationException(
                    $"Ingredient(s) not found: {string.Join(", ", missingIds)}");
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
