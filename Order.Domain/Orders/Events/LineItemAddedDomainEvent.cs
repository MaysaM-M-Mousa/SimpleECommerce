using Order.Domain.Primitives;

namespace Order.Domain.Orders.Events;

public record LineItemAddedDomainEvent(
    Guid OrderId, 
    int ProductId,
    int Quantity,
    decimal Price) : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
