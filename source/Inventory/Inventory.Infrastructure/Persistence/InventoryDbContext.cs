using BuildingBlocks.Application.Inbox;
using BuildingBlocks.Application.Outbox;
using Inventory.Application.Products.ReserveStock.Saga;
using Inventory.Domain.Products;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
        : base(options) { }

    public DbSet<Product> Products { get; set; }

    public DbSet<InboxMessage> InboxMessages { get; set; }

    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    public DbSet<ReserveStocksSagaState> ReserveStocksSagaStates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventoryDbContext).Assembly);

        // MassTransit outbox & inbox tables
        modelBuilder.AddInboxStateEntity(opts => opts.ToTable("MassTransit_InboxState"));
        modelBuilder.AddOutboxStateEntity(opts => opts.ToTable("MassTransit_OutboxState"));
        modelBuilder.AddOutboxMessageEntity(opts => opts.ToTable("MassTransit_OutboxMessage"));
    }
}
