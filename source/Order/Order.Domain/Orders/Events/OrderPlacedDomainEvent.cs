﻿using BuildingBlocks.Domain;

namespace Order.Domain.Orders.Events;

public record OrderPlacedDomainEvent(
    Guid OrderId,
    Guid CustomerId,
    decimal TotalAmount,
    List<Item> Items)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public record Item(int ProductId, int Quantity);