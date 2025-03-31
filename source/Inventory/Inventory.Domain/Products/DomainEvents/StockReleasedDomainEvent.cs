using BuildingBlocks.Domain;

namespace Inventory.Domain.Products.DomainEvents;

public record StockReleasedDomainEvent(
    int ProductId,
    int Quantity,
    Guid OrderId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
