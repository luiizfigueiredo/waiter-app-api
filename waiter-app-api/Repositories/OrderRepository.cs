using Microsoft.EntityFrameworkCore;
using WaiterApp.Data;
using WaiterApp.Models;
using WaiterApp.Repositories.Interfaces;

namespace WaiterApp.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllAsync()
        => await _context.Orders
            .AsNoTracking()
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ThenInclude(p => p!.Category)
            .AsSplitQuery()
            .OrderBy(o => o.CreatedAt)
            .ToListAsync();

    public async Task<Order?> GetByIdAsync(Guid id)
        => await _context.Orders
            .AsNoTracking()
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);

    public async Task<Order> CreateAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task UpdateStatusAsync(Guid id, string status)
    {
        await _context.Orders
            .Where(o => o.Id == id)
            .ExecuteUpdateAsync(setters => setters.SetProperty(o => o.Status, status));
    }

    public async Task DeleteAsync(Guid id)
    {
        await _context.Orders
            .Where(o => o.Id == id)
            .ExecuteDeleteAsync();
    }
}
