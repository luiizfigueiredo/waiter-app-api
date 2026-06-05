using Microsoft.AspNetCore.SignalR;
using WaiterApp.DTOs;
using WaiterApp.Hubs;
using WaiterApp.Models;

namespace WaiterApp.Notifications;

public class OrderNotifier : IOrderNotifier
{
    private readonly IHubContext<OrderHub> _hub;

    public OrderNotifier(IHubContext<OrderHub> hub)
    {
        _hub = hub;
    }

    public Task OrderCreatedAsync(Order order)
    {
        var dto = order.ToResponse();
        return _hub.Clients
            .Groups(OrderHub.KitchenGroup, OrderHub.CashierGroup)
            .SendAsync("order:created", dto);
    }

    public async Task OrderStatusChangedAsync(Order order)
    {
        var dto = order.ToResponse();
        await _hub.Clients
            .Groups(OrderHub.KitchenGroup, OrderHub.CashierGroup, OrderHub.TableGroup(order.Table))
            .SendAsync("order:statusChanged", dto);

        if (order.Status == "DONE")
        {
            await _hub.Clients
                .Group(OrderHub.TableGroup(order.Table))
                .SendAsync("order:done", new { id = order.Id, table = order.Table });
        }
    }

    public Task OrderDeletedAsync(Guid orderId, string table)
    {
        return _hub.Clients
            .Groups(OrderHub.KitchenGroup, OrderHub.CashierGroup)
            .SendAsync("order:deleted", new { id = orderId });
    }
}
