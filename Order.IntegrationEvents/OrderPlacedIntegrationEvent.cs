using BuildingBlocks.IntegrationEvent;

namespace Order.IntegrationEvents;

public class OrderPlacedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
    
    public Guid CustomerId { get; set; }

    public DateTime OccurredOnUtc { get; set; }

    public decimal TotalAmount { get; set; }

    public List<Item> Items { get; set; } = new();
}

public record Item(int ProductId, int Quantity);