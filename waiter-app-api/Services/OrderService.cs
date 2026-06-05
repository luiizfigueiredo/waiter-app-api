using WaiterApp.Models;
using WaiterApp.Repositories.Interfaces;
using WaiterApp.Services.Interfaces;

namespace WaiterApp.Services;

public class OrderService : IOrderService
{
    private static readonly HashSet<string> ValidStatuses = new() { "WAITING", "IN_PRODUCTION", "DONE" };

    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _orderRepository.GetAllAsync();
    }

    public async Task<Order> CreateAsync(string table, List<(Guid ProductId, int Quantity)> items)
    {
        if (items.Count == 0)
            throw new ArgumentException("Order must contain at least one product.", nameof(items));

        var productIds = items.Select(i => i.ProductId).Distinct().ToList();
        var existingProducts = await _productRepository.GetByIdsAsync(productIds);
        var existingProductIds = existingProducts.Select(p => p.Id).ToHashSet();

        var missingProducts = productIds.Where(id => !existingProductIds.Contains(id)).ToList();
        if (missingProducts.Count > 0)
            throw new InvalidOperationException($"Products not found: {string.Join(", ", missingProducts)}");

        var orderItems = items.Select(i => new OrderItem
        {
            ProductId = i.ProductId,
            Quantity = i.Quantity
        }).ToList();

        var order = new Order
        {
            Table = table,
            OrderItems = orderItems
        };

        return await _orderRepository.CreateAsync(order);
    }

    public async Task UpdateStatusAsync(Guid orderId, string status)
    {
        if (!ValidStatuses.Contains(status))
            throw new ArgumentException($"Status must be one of: {string.Join(", ", ValidStatuses)}.", nameof(status));

        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order is null)
            throw new InvalidOperationException($"Order with id '{orderId}' not found.");

        await _orderRepository.UpdateStatusAsync(orderId, status);
    }

    public async Task DeleteAsync(Guid orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order is null)
            throw new InvalidOperationException($"Order with id '{orderId}' not found.");

        await _orderRepository.DeleteAsync(orderId);
    }
}
