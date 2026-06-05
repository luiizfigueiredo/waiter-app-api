namespace WaiterApp.DTOs;

public class CreateOrderItemRequest
{
    public string Product { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
}

public class CreateOrderRequest
{
    public string Table { get; set; } = string.Empty;
    public List<CreateOrderItemRequest> Products { get; set; } = new();
}

public class UpdateOrderStatusRequest
{
    public string Status { get; set; } = string.Empty;
}

public class OrderItemResponse
{
    public ProductResponse Product { get; set; } = null!;
    public int Quantity { get; set; }
}

public class OrderResponse
{
    public Guid Id { get; set; }
    public string Table { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<OrderItemResponse> Products { get; set; } = new();
}
