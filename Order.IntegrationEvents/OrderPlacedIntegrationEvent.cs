namespace Order.IntegrationEvents;

public class OrderPlacedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
    
    public Guid CustomerId { get; set; }

    public DateTime OccurredOnUtc { get; set; }

    public decimal TotalAmount { get; set; }

    public List<(int ProductId, int Quantity)> Items { get; set; } = new();
}
