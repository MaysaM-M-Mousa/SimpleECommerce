using FluentAssertions;
using FluentAssertions.Execution;
using Inventory.Domain.Products;
using Inventory.Domain.Products.DomainEvents;

namespace Inventory.Domain.Tests.Products;

public class DeductTests
{
    [Fact]
    public void Deducting_PositiveQuantity_DecrementsStockLevels_AndRemovesReservation()
    {
        // Arrange
        var product = Product.Create(id: 1, name: "product name", description: "product description", quantity: 10, price: 100m);
        var orderId = Guid.NewGuid();
        product.Reserve(orderId, 2);
        product.ClearDomainEvents();

        // Act
        product.Deduct(orderId, 2);

        // Assert
        using (new AssertionScope())
        {
            product.StockQuantity.Should().Be(8);
            product.ReservedQuantity.Should().Be(0);
            product.AvailableQuantity.Should().Be(8);
            product.Reservations.Should().BeEmpty();
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
        var act = () => product.Deduct(Guid.NewGuid(), quantityToDeduct);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Quantity must be positive!");
    }

    [Fact]
    public void DeductingQuantity_Exceeds_StockLevels_Fails()
    {
        // Arrange
        var product = Product.Create(id: 1, name: "product name", description: "product description", quantity: 10, price: 100m);
        product.ClearDomainEvents();

        // Act
        var act = () => product.Deduct(Guid.NewGuid(), 12);

        // Assert
        act.Should().Throw<Exception>().WithMessage("No enough stocks to deduct!");
    }

    [Fact]
    public void Deduction_ForUnreservedOrder_Fails()
    {
        // Arrange
        var product = Product.Create(id: 1, name: "product name", description: "product description", quantity: 10, price: 100m);
        product.ClearDomainEvents();

        // Act
        var act = () => product.Deduct(Guid.NewGuid(), 2);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Order has no reservation!");
    }

    [Fact]
    public void Deducting_DifferentQuantity_ThanReserved_Fails()
    {
        // Arrange
        var product = Product.Create(id: 1, name: "product name", description: "product description", quantity: 10, price: 100m);
        var orderId = Guid.NewGuid();
        product.Reserve(orderId, 2);
        product.ClearDomainEvents();

        // Act
        var act = () => product.Deduct(orderId, 4);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Deduction quantity must match order's reserved quantity!");
    }
}
