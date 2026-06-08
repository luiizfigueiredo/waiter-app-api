using Microsoft.AspNetCore.Mvc;
using WaiterApp.DTOs;
using WaiterApp.Models;
using WaiterApp.Services.Interfaces;

namespace WaiterApp.Controllers;

[ApiController]
[Route("ingredients")]
public class IngredientsController : ControllerBase
{
    private readonly IIngredientService _ingredientService;

    public IngredientsController(IIngredientService ingredientService)
    {
        _ingredientService = ingredientService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var ingredients = await _ingredientService.GetAllAsync();
        return Ok(ingredients.Select(MapToResponse));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var ingredient = await _ingredientService.GetByIdAsync(id);
        if (ingredient is null)
            return NotFound();

        return Ok(MapToResponse(ingredient));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateIngredientRequest request)
    {
        var ingredient = await _ingredientService.CreateAsync(request.Name, request.Icon);
        return StatusCode(201, MapToResponse(ingredient));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateIngredientRequest request)
    {
        var ingredient = await _ingredientService.UpdateAsync(id, request.Name, request.Icon);
        return Ok(MapToResponse(ingredient));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _ingredientService.DeleteAsync(id);
        return NoContent();
    }

    private static IngredientDto MapToResponse(Ingredient i) => new()
    {
        Id = i.Id,
        Name = i.Name,
        Icon = i.Icon
    };
}
