using BuildingBlocks.Domain;
using Inventory.Domain.Products.DomainEvents;

namespace Inventory.Domain.Products;

public class Product : AggregateRoot<int>
{    
    public string Name { get; private set; }
    
    public string Description { get; private set; }

    public int StockQuantity { get; private set; }

    public decimal Price { get; private set; }

    private Dictionary<Guid, Reservation> _reservationsByOrderId =>
                _reservations.ToDictionary(r => r.OrderId);

    private List<Reservation> _reservations = new();

    public IReadOnlyList<Reservation> Reservations =>
        _reservations.AsReadOnly();

    private Product()
    {
        // EF Core Only
    }

    private Product(int id, string name, string description, int quantity, decimal price) 
        : base(id)
    {
        Name = name;
        Description = description;
        StockQuantity = quantity;
        Price = price;
    }

    private bool CanDeduct(int quantity) 
        => StockQuantity >= quantity;

    public int ReservedQuantity =>
        _reservationsByOrderId.Sum(x => x.Value.Quantity);

    public int AvailableQuantity =>
        StockQuantity - ReservedQuantity;

    public void Reserve(Guid orderId, int quantity)
    {
        if (quantity > AvailableQuantity)
        {
            throw new Exception("No enough available stocks to reserve!");
        }

        if (_reservationsByOrderId.ContainsKey(orderId))
        {
            throw new Exception("Order already has a reservation!");
        }

        _reservations.Add(Reservation.Create(Id, orderId, quantity));

        RaiseDomainEvent(new StockReservedDomainEvent(Id, quantity, orderId));
    }

    public void Release(Guid orderId, int quantity)
    {
        if (quantity <= 0)
        {
            throw new Exception("Quantity must not be positive!");
        }

        if (!_reservationsByOrderId.TryGetValue(orderId, out var orderReservation))
        {
            throw new Exception("Order has no reservation!");
        }

        if (quantity != orderReservation.Quantity)
        {
            throw new Exception("Released quantity does not match order reservation's quantity!");
        }

        RemoveReservation(orderId);

        RaiseDomainEvent(new StockReleasedDomainEvent(Id, quantity, orderId));
    }

    public void Deduct(Guid orderId, int quantity)
    {
        if (quantity <= 0)
        {
            throw new Exception("Quantity must be positive!");
        }

        if (!CanDeduct(quantity))
        {
            throw new Exception("No enough stocks to deduct!");
        }

        if (!_reservationsByOrderId.TryGetValue(orderId, out var orderReservation))
        {
            throw new Exception("Order has no reservation!");
        }

        if (quantity != orderReservation.Quantity)
        {
            throw new Exception("Deduction quantity must match order's reserved quantity!");
        }

        StockQuantity -= quantity;
        RemoveReservation(orderId);

        RaiseDomainEvent(new StockDeductedDomainEvent(Id, quantity, orderId));
    }

    private void RemoveReservation(Guid orderId)
    {
        var orderReservation = _reservations.First(r => r.OrderId == orderId);
        _reservations.Remove(orderReservation);
    }

    public static Product Create(int id, string name, string description, int quantity, decimal price)
    {
        var product = new Product(id, name, description, quantity, price);

        product.RaiseDomainEvent(new ProductCreatedDomainEvent(id, name, description, quantity, price));

        return product;
    }
}
