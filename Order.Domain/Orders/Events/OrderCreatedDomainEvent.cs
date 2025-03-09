using BuildingBlocks.Domain;

namespace Order.Domain.Orders.Events;

public record OrderCreatedDomainEvent(
    Guid OrderId, 
    Guid CustomerId) 
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
