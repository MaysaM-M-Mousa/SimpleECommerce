using BuildingBlocks.Application.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Order.Persistence;

public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) 
        : base(options) { }

    public DbSet<Domain.Orders.Order> Orders { get; set; }

    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrdersDbContext).Assembly);
    }
}
