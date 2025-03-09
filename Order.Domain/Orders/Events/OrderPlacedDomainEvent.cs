using BuildingBlocks.Domain;

namespace Order.Domain.Orders.Events;

public record OrderPlacedDomainEvent(
    Guid OrderId,
    Guid CustomerId,
    decimal TotalAmount,
    List<(int ProductId, int Quantity)> Items)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
