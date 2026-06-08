namespace WaiterApp.Models;

public class Ingredient
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
