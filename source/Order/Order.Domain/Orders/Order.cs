using BuildingBlocks.Domain;
using Order.Domain.Orders.Events;

namespace Order.Domain.Orders;

public class Order : AggregateRoot<Guid>
{
    private List<LineItem> _lineItems = new();

    public string? Description { get; private set; }

    public decimal TotalAmount { get; private set; }

    public OrderStatus Status { get; private set; }

    public Guid CustomerId { get; private set; }

    public IReadOnlyList<LineItem> LineItems => _lineItems.AsReadOnly();

    private Order(Guid id, string? description, Guid customerId)
        : base(id)
    {
        Description = description;
        CustomerId = customerId;
        Status = OrderStatus.Created;
    }

    public static Order Create(string? description, Guid customerId)
    {
        var order = new Order(Guid.NewGuid(), description, customerId);

        order.RaiseDomainEvent(new OrderCreatedDomainEvent(order.Id, order.CustomerId));

        return order;
    }

    public void AddLineItem(int productId, int quantity, decimal price)
    {
        if (Status == OrderStatus.Cancel)
        {
            throw new InvalidOperationException("Can't add items to cancelled order!");
        }

        if (Status == OrderStatus.Placed)
        {
            throw new InvalidOperationException("Can't add items to placed order!");
        }

        var lineItem = _lineItems.FirstOrDefault(li => li.ProductId == productId);

        if (lineItem is not null)
        {
            lineItem.IncreaseQuantity(quantity);
        }
        else
        {
            _lineItems.Add(LineItem.Create(
                id: Guid.Empty,
                productId: productId,
                quantity: quantity,
                price: price,
                orderId: Id));
        }

        RecalculateTotalAmount();

        RaiseDomainEvent(new LineItemAddedDomainEvent(Id, productId, quantity, price));
    }

    public void PlaceOrder()
    {
        if (Status == OrderStatus.Placed)
        {
            throw new InvalidOperationException("Order is already placed!");
        }

        if (Status == OrderStatus.Cancel)
        {
            throw new InvalidOperationException("Can't place a cancelled order!");
        }

        if (_lineItems.Count == 0)
        {
            throw new InvalidOperationException("Can't place an empty order!");
        }

        Status = OrderStatus.Placed;

        RaiseDomainEvent(new OrderPlacedDomainEvent(Id, CustomerId, TotalAmount, LineItems.Select(li => new Item(li.ProductId, li.Quantity)).ToList()));
    }

    public void Cancel(string? reason)
    {
        if (Status == OrderStatus.Cancel)
        {
            throw new InvalidOperationException("Already Cancelled!");
        }

        var isPlaced = Status == OrderStatus.Placed;

        Status = OrderStatus.Cancel;

        RaiseDomainEvent(new OrderCancelledDomainEvent(Id, reason, isPlaced, LineItems.Select(li => new Item(li.ProductId, li.Quantity)).ToList()));
    }

    private void RecalculateTotalAmount()
    {
        var totAmount = 0.0m;

        _lineItems.ForEach(li =>
        {
            totAmount += li.CalculateTotalPrice();
        });

        TotalAmount = totAmount;
    }
}

