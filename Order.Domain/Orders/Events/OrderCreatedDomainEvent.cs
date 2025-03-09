using Order.Domain.Primitives;

namespace Order.Domain.Orders.Events;

public record OrderCreatedDomainEvent(
    Guid OrderId, 
    Guid CustomerId) 
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
