using BuildingBlocks.IntegrationEvent;

namespace Order.IntegrationEvents;

public class OrderCancelledIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }

    public string? Reason { get; set; }

    public bool IsPlaced { get; set; }

    public List<Item> Items { get; set; } = new();

    public DateTime OccurredOnUtc { get; set; }
}
