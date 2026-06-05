using WaiterApp.Models;

namespace WaiterApp.Services.Interfaces;

public interface IOrderService
{
    Task<List<Order>> GetAllAsync();
    Task<Order> CreateAsync(string table, List<(Guid ProductId, int Quantity)> items);
    Task UpdateStatusAsync(Guid orderId, string status);
    Task DeleteAsync(Guid orderId);
}
