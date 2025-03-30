namespace Inventory.Domain.Products;

public class Reservation
{
    public long Id { get; private set; }

    public int ProductId { get; private set; }

    public Guid OrderId { get; private set; }

    public int Quantity { get; private set; }

    private Reservation()
    {
        // Only EF Core
    }

    private Reservation(int productId, Guid orderId, int quantity)
    {
        ProductId = productId;
        OrderId = orderId;
        Quantity = quantity;
    }

    public static Reservation Create(int productId, Guid orderId, int quantity)
        => new Reservation(productId, orderId, quantity);
}
