using Order.Domain.Primitives;

namespace Order.Domain.Orders.Events;

internal record OrderCancelledDomainEvent(
    Guid OrderId, 
    string? Reason,
    bool IsPlaced) : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
