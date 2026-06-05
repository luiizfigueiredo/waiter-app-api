using WaiterApp.Models;

namespace WaiterApp.DTOs;

public static class OrderMapper
{
    public static OrderResponse ToResponse(this Order order)
    {
        return new OrderResponse
        {
            Id = order.Id,
            Table = order.Table,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            Products = order.OrderItems.Select(oi => new OrderItemResponse
            {
                Product = new ProductResponse
                {
                    Id = oi.Product.Id,
                    Name = oi.Product.Name,
                    Description = oi.Product.Description,
                    ImagePath = oi.Product.ImagePath,
                    Price = oi.Product.Price,
                    Ingredients = oi.Product.Ingredients.Select(i => new IngredientDto { Name = i.Name, Icon = i.Icon }).ToList(),
                    Category = oi.Product.Category is not null ? new CategoryResponse
                    {
                        Id = oi.Product.Category.Id,
                        Name = oi.Product.Category.Name,
                        Icon = oi.Product.Category.Icon
                    } : null
                },
                Quantity = oi.Quantity
            }).ToList()
        };
    }
}
