namespace Order.Domain.Orders;

public class LineItem
{
    public Guid Id { get; private set; }

    public int ProductId { get; private set; }

    public decimal Price { get; private set; }

    public int Quantity { get; private set; }

    public Guid OrderId { get; private set; }

    internal LineItem(Guid id, int productId, decimal price, int quantity, Guid orderId)
    {
        Id = id;
        ProductId = productId;
        Price = price;
        Quantity = quantity;
        OrderId = orderId;
    }

    private LineItem()
    {
        // Only EF Core
    }

    public void IncreaseStockQuantity(int additionalQuantity)
    {
        if (additionalQuantity <= 0) 
        {
            throw new ArgumentException("Quantity must be positive!");
        }
        Quantity += additionalQuantity;
    }

    public decimal CalculateTotalPrice() => Quantity * Price;
}
