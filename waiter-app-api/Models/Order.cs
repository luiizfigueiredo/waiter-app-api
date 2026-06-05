namespace WaiterApp.Models;

public class Order
{
    public Guid Id { get; set; }
    public string Table { get; set; } = string.Empty;
    public string Status { get; set; } = "WAITING";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
