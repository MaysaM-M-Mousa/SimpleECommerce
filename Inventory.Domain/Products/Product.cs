namespace Inventory.Domain.Products;

public class Product
{
    public int Id { get; private set; }
    
    public string Name { get; private set; }
    
    public string Description { get; private set; }

    public int Quantity { get; private set; }

    public decimal Price { get; private set; }

    private Product(int id, string name, string description, int quantity, decimal price)
    {
        Id = id;
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

        // RaiseDomainEvent()
    }

    public static Product Create(int id, string name, string description, int quantity, decimal price)
    {
        var product = new Product(id, name, description, quantity, price);

        return product;
    }
}
