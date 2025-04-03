using BuildingBlocks.IntegrationEvent;

namespace Inventory.IntegrationEvents;

public class StocksReservationFailedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; init; }

    public string? Reason { get; init; }
}