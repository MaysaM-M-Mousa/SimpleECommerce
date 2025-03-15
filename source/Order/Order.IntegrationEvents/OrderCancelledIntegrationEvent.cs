using BuildingBlocks.IntegrationEvent;

namespace Order.IntegrationEvents;

public class OrderCancelledIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; }

    public string? Reason { get; }

    public bool IsPlaced { get; }

    public List<Item> Items { get; } = new();

    public DateTime OccurredOnUtc { get; set; }
}
