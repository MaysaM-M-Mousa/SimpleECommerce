namespace Order.Domain.Orders;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id);

    void AddOrder(Order order);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
