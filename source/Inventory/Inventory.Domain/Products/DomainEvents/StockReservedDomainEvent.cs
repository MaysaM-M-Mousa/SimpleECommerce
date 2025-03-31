using BuildingBlocks.Domain;

namespace Inventory.Domain.Products.DomainEvents;

public record StockReservedDomainEvent(
    int ProductId,
    int Quantity,
    Guid OrderId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
