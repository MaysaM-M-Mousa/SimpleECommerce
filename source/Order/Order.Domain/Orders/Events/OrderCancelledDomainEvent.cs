using BuildingBlocks.Domain;

namespace Order.Domain.Orders.Events;

public record OrderCancelledDomainEvent(
    Guid OrderId, 
    string? Reason,
    bool IsPlaced,
    List<Item> Items) : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
