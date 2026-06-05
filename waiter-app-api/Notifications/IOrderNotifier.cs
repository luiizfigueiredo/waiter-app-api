using WaiterApp.Models;

namespace WaiterApp.Notifications;

public interface IOrderNotifier
{
    Task OrderCreatedAsync(Order order);
    Task OrderStatusChangedAsync(Order order);
    Task OrderDeletedAsync(Guid orderId, string table);
}
