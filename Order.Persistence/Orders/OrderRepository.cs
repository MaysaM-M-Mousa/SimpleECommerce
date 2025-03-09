using Microsoft.EntityFrameworkCore;
using Order.Domain.Orders;

namespace Order.Persistence.Orders;

internal class OrderRepository : IOrderRepository
{
    private readonly OrdersDbContext _context;

    public OrderRepository(OrdersDbContext context)
    {
        _context = context;
    }

    public void AddOrder(Domain.Orders.Order order)
    {
        _context.Orders.Add(order);
    }

    public async Task<Domain.Orders.Order?> GetByIdAsync(Guid id)
    {
        return await _context
            .Orders
            .Include(s => s.LineItems)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
