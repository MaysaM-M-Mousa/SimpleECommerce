using BuildingBlocks.Domain;

namespace Inventory.Domain.Products.DomainEvents;

public record StockReservedDomainEvent(
    int ProductId,
    int Quantity)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
