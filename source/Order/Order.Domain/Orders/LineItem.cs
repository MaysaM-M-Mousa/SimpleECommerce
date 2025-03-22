namespace Order.Domain.Orders;

public class LineItem
{
    public Guid Id { get; private set; }

    public int ProductId { get; private set; }

    public decimal Price { get; private set; }

    public int Quantity { get; private set; }

    public Guid OrderId { get; private set; }

    private LineItem(Guid id, int productId, decimal price, int quantity, Guid orderId)
    {
        Id = id;
        ProductId = productId;
        Price = price;
        Quantity = quantity;
        OrderId = orderId;
    }

    internal static LineItem Create(Guid id, int productId, decimal price, int quantity, Guid orderId)
    {
        var lineItem = new LineItem(id, productId, price, quantity, orderId);

        return lineItem;
    }

    private LineItem()
    {
        // Only EF Core
    }

    public void IncreaseQuantity(int additionalQuantity)
    {
        if (additionalQuantity <= 0) 
        {
            throw new ArgumentException("Quantity must be positive!");
        }

        Quantity += additionalQuantity;
    }

    public decimal CalculateTotalPrice() => Quantity * Price;
}
