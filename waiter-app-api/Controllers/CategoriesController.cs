using Microsoft.AspNetCore.Mvc;
using WaiterApp.DTOs;
using WaiterApp.Models;
using WaiterApp.Services.Interfaces;

namespace WaiterApp.Controllers;

[ApiController]
[Route("categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();
        return Ok(categories.Select(MapToResponse));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
    {
        var category = await _categoryService.CreateAsync(request.Name, request.Icon);
        return StatusCode(201, MapToResponse(category));
    }

    [HttpGet("{categoryId}/products")]
    public async Task<IActionResult> GetProducts(string categoryId)
    {
        if (!Guid.TryParse(categoryId, out var id))
            return BadRequest(new { error = "Invalid category id" });

        var (products, category) = await _categoryService.GetProductsByCategoryAsync(id);
        var categoryResponse = category is not null ? MapToResponse(category) : null;

        return Ok(products.Select(p => MapProductToResponse(p, categoryResponse)));
    }

    private static CategoryResponse MapToResponse(Category c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Icon = c.Icon
    };

    private static ProductResponse MapProductToResponse(Product p, CategoryResponse? category) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        ImagePath = p.ImagePath,
        Price = p.Price,
        Ingredients = p.Ingredients.Select(i => new IngredientDto { Id = i.Id, Name = i.Name, Icon = i.Icon }).ToList(),
        Category = category
    };
}
