using FluentAssertions;
using FluentAssertions.Execution;
using Inventory.Domain.Products;
using Inventory.Domain.Products.DomainEvents;

namespace Inventory.Domain.Tests.Products;

public class ReleaseTests
{
    [Fact]
    public void Releasing_PositiveQuantity_IncrementsStockLevels()
    {
        // Arrange
        var product = Product.Create(id: 1, name: "product name", description: "product description", quantity: 10, price: 100m);
        product.ClearDomainEvents();

        // Act
        product.Release(2);

        // Assert
        using (new AssertionScope())
        {
            product.Quantity.Should().Be(12);
            product.GetDomainEvents().Should().ContainSingle(e => e is StockReleasedDomainEvent);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-2)]
    public void Releasing_NegativeOrZero_Quantity_Fails(int quantityToRelease)
    {
        // Arrange
        var product = Product.Create(id: 1, name: "product name", description: "product description", quantity: 10, price: 100m);
        product.ClearDomainEvents();

        // Act
        var act = () => product.Release(quantityToRelease);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Quantity must not be positive!");
    }
}
