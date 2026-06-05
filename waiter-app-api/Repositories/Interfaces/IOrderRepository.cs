using WaiterApp.Models;

namespace WaiterApp.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<List<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(Guid id);
    Task<Order> CreateAsync(Order order);
    Task UpdateStatusAsync(Guid id, string status);
    Task DeleteAsync(Guid id);
}
