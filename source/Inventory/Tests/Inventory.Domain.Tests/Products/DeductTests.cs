using FluentAssertions;
using FluentAssertions.Execution;
using Inventory.Domain.Products;
using Inventory.Domain.Products.DomainEvents;

namespace Inventory.Domain.Tests.Products;

public class DeductTests
{
    [Fact]
    public void Deducting_PositiveQuantity_DecrementsStockLevels()
    {
        // Arrange
        var product = Product.Create(id: 1, name: "product name", description: "product description", quantity: 10, price: 100m);
        product.ClearDomainEvents();

        // Act
        product.Deduct(2);

        // Assert
        using (new AssertionScope())
        {
            product.Quantity.Should().Be(8);
            product.GetDomainEvents().Should().ContainSingle(e => e is StockDeductedDomainEvent);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-2)]
    public void Deducting_NegativeOrZeroQuantity_Fails(int quantityToDeduct)
    {
        // Arrange
        var product = Product.Create(id: 1, name: "product name", description: "product description", quantity: 10, price: 100m);
        product.ClearDomainEvents();

        // Act
        var act = () => product.Deduct(quantityToDeduct);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Quantity must be positive!");
    }

    [Fact]
    public void DeductingQuantity_Exceeds_AvailableStockLevels_Fails()
    {
        // Arrange
        var product = Product.Create(id: 1, name: "product name", description: "product description", quantity: 10, price: 100m);
        product.ClearDomainEvents();

        // Act
        var act = () => product.Deduct(12);

        // Assert
        act.Should().Throw<Exception>().WithMessage("No enough stocks to deduct!");

    }
}
