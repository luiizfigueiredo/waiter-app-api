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

        var response = orders.Select(o => o.ToResponse()).ToList();

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        var items = request.Products.Select(p => (Guid.Parse(p.Product), p.Quantity)).ToList();

        var order = await _orderService.CreateAsync(request.Table, items);

        return StatusCode(201, order.ToResponse());
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
