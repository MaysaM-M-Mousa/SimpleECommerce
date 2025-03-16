using BuildingBlocks.Domain;
using Inventory.Domain.Products.DomainEvents;

namespace Inventory.Domain.Products;

public class Product : AggregateRoot<int>
{    
    public string Name { get; private set; }
    
    public string Description { get; private set; }

    public int Quantity { get; private set; }

    public decimal Price { get; private set; }

    private Product(int id, string name, string description, int quantity, decimal price) 
        : base(id)
    {
        Name = name;
        Description = description;
        Quantity = quantity;
        Price = price;
    }

    public bool CanDeduct(int quantity) 
        => Quantity >= quantity;

    public void Deduct(int quantity)
    {
        Quantity-= quantity;

        RaiseDomainEvent(new StockDeductedDomainEvent(Id, quantity));
    }

    public void Release(int quantity)
    {
        Quantity += quantity;

        RaiseDomainEvent(new StockReleasedDomainEvent(Id, quantity));
    }

    public static Product Create(int id, string name, string description, int quantity, decimal price)
    {
        var product = new Product(id, name, description, quantity, price);

        product.RaiseDomainEvent(new ProductCreatedDomainEvent(id, name, description, quantity, price));

        return product;
    }
}
