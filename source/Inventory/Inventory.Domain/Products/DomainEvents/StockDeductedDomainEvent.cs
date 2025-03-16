using BuildingBlocks.Domain;

namespace Inventory.Domain.Products.DomainEvents;

public record StockDeductedDomainEvent(
    int ProductId, 
    int Quantity) 
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
