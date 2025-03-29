using FluentAssertions;
using FluentAssertions.Execution;
using Inventory.Domain.Products;
using Inventory.Domain.Products.DomainEvents;

namespace Inventory.Domain.Tests.Products;

public class CreateTests
{
    [Fact]
    public void CreateProduct_Succeeds()
    {
        // Arrange
        var id = 1;
        var name = "Product Name";
        var description = "Product Description";
        var quantity = 10;
        var price = 100.0m;

        // Act
        var product = Product.Create(id, name, description, quantity, price); 

        // Assert
        using (new AssertionScope())
        {
            product.Should().NotBeNull();
            product.Id.Should().Be(id);
            product.Name.Should().Be(name);
            product.Quantity.Should().Be(quantity);
            product.Description.Should().Be(description);
            product.Price.Should().Be(price);
            product.GetDomainEvents().Should().ContainSingle(e => e is ProductCreatedDomainEvent);
        }
    }
}
