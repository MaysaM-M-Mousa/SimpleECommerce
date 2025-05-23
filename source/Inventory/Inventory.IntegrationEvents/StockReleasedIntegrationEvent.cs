﻿using BuildingBlocks.IntegrationEvent;

namespace Inventory.IntegrationEvents;

public class StockReleasedIntegrationEvent : IntegrationEvent
{
    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public Guid OrderId { get; set; }

    public DateTime OccurredOnUtc { get; set; }
}