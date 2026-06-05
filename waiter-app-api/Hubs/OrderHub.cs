using Microsoft.AspNetCore.SignalR;

namespace WaiterApp.Hubs;

public class OrderHub : Hub
{
    public const string KitchenGroup = "kitchen";
    public const string CashierGroup = "cashier";

    public static string TableGroup(string table) => $"table-{table}";

    public Task JoinKitchen()
        => Groups.AddToGroupAsync(Context.ConnectionId, KitchenGroup);

    public Task JoinCashier()
        => Groups.AddToGroupAsync(Context.ConnectionId, CashierGroup);

    public Task JoinTable(string table)
        => Groups.AddToGroupAsync(Context.ConnectionId, TableGroup(table));

    public Task LeaveTable(string table)
        => Groups.RemoveFromGroupAsync(Context.ConnectionId, TableGroup(table));
}
