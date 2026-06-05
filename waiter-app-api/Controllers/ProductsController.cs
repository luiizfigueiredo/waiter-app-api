using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using WaiterApp.DTOs;
using WaiterApp.Models;
using WaiterApp.Services.Interfaces;

namespace WaiterApp.Controllers;

[ApiController]
[Route("products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IWebHostEnvironment _env;

    public ProductsController(
        IProductService productService,
        IWebHostEnvironment env)
    {
        _productService = productService;
        _env = env;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products.Select(MapToResponse));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateProductRequest request)
    {
        var categoryId = Guid.Parse(request.Category);
        var price = decimal.Parse(request.Price, NumberStyles.Any, CultureInfo.InvariantCulture);

        var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
        Directory.CreateDirectory(uploadsPath);

        var filename = $"{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}-{request.Image!.FileName}";
        var filePath = Path.Combine(uploadsPath, filename);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.Image.CopyToAsync(stream);
        }

        var product = await _productService.CreateAsync(
            request.Name,
            request.Description,
            price,
            filename,
            categoryId,
            request.Ingredients);

        return StatusCode(201, MapToResponse(product));
    }

    private static ProductResponse MapToResponse(Product p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        ImagePath = p.ImagePath,
        Price = p.Price,
        Ingredients = p.Ingredients.Select(i => new IngredientDto { Name = i.Name, Icon = i.Icon }).ToList(),
        Category = p.Category is not null ? new CategoryResponse
        {
            Id = p.Category.Id,
            Name = p.Category.Name,
            Icon = p.Category.Icon
        } : null
    };
}
