using Microsoft.AspNetCore.Mvc;
using WaiterApp.DTOs;
using WaiterApp.Services.Interfaces;

namespace WaiterApp.Controllers;

[ApiController]
[Route("orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _orderService.GetAllAsync();

        var response = orders.Select(o => new OrderResponse
        {
            Id = o.Id,
            Table = o.Table,
            Status = o.Status,
            CreatedAt = o.CreatedAt,
            Products = o.OrderItems.Select(oi => new OrderItemResponse
            {
                Product = new ProductResponse
                {
                    Id = oi.Product.Id,
                    Name = oi.Product.Name,
                    Description = oi.Product.Description,
                    ImagePath = oi.Product.ImagePath,
                    Price = oi.Product.Price,
                    Ingredients = oi.Product.Ingredients.Select(i => new IngredientDto { Name = i.Name, Icon = i.Icon }).ToList(),
                    Category = oi.Product.Category is not null ? new CategoryResponse
                    {
                        Id = oi.Product.Category.Id,
                        Name = oi.Product.Category.Name,
                        Icon = oi.Product.Category.Icon
                    } : null
                },
                Quantity = oi.Quantity
            }).ToList()
        }).ToList();

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        var items = request.Products.Select(p => (Guid.Parse(p.Product), p.Quantity)).ToList();

        var order = await _orderService.CreateAsync(request.Table, items);

        return StatusCode(201, new OrderResponse
        {
            Id = order.Id,
            Table = order.Table,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            Products = order.OrderItems.Select(oi => new OrderItemResponse
            {
                Product = new ProductResponse { Id = oi.ProductId },
                Quantity = oi.Quantity
            }).ToList()
        });
    }

    [HttpPatch("{orderId}")]
    public async Task<IActionResult> UpdateStatus(string orderId, [FromBody] UpdateOrderStatusRequest request)
    {
        if (!Guid.TryParse(orderId, out var id))
            return BadRequest(new { error = "Invalid order id" });

        await _orderService.UpdateStatusAsync(id, request.Status);
        return NoContent();
    }

    [HttpDelete("{orderId}")]
    public async Task<IActionResult> Delete(string orderId)
    {
        if (!Guid.TryParse(orderId, out var id))
            return BadRequest(new { error = "Invalid order id" });

        await _orderService.DeleteAsync(id);
        return NoContent();
    }
}
