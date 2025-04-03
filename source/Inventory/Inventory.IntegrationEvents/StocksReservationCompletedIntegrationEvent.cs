using BuildingBlocks.IntegrationEvent;

namespace Inventory.IntegrationEvents;

public class StocksReservationCompletedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; init; }
}
