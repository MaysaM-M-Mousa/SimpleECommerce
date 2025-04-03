using BuildingBlocks.IntegrationEvent;

namespace Inventory.IntegrationEvents;

public class ReservationTimeoutExpiredIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; init; }
}
