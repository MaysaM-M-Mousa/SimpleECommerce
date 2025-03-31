using FluentAssertions;
using FluentAssertions.Execution;
using Inventory.Domain.Products;
using Inventory.Domain.Products.DomainEvents;

namespace Inventory.Domain.Tests.Products;

public class ReleaseTests
{
    [Fact]
    public void Releasing_PositiveQuantity_RemovesOrderReservation()
    {
        // Arrange
        var product = Product.Create(id: 1, name: "product name", description: "product description", quantity: 10, price: 100m);
        var orderId = Guid.NewGuid();
        product.Reserve(orderId, 2);
        product.ClearDomainEvents();

        // Act
        product.Release(orderId, 2);

        // Assert
        using (new AssertionScope())
        {
            product.AvailableQuantity.Should().Be(10);
            product.ReservedQuantity.Should().Be(0);
            product.Reservations.Should().BeEmpty();
            product.GetDomainEvents().Should().ContainSingle(e => e is StockReleasedDomainEvent);
        }
    }

    [Fact]
    public void Releasing_DifferentQuantity_Than_OrderReservationQuantity_Fails()
    {
        // Arrange
        var product = Product.Create(id: 1, name: "product name", description: "product description", quantity: 10, price: 100m);
        var orderId = Guid.NewGuid();
        product.Reserve(orderId, 2);
        product.ClearDomainEvents();

        // Act
        var act = () => product.Release(orderId, 3);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Released quantity does not match order reservation's quantity!");
    }

    [Fact]
    public void Releasing_UnreservedOrder_Fails()
    {
        // Arrange
        var product = Product.Create(id: 1, name: "product name", description: "product description", quantity: 10, price: 100m);
        product.Reserve(Guid.NewGuid(), 4);
        product.ClearDomainEvents();
        var randomOrderId = Guid.NewGuid();

        // Act
        var act = () => product.Release(randomOrderId, 2);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Order has no reservation!");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-2)]
    public void Releasing_NegativeOrZero_Quantity_Fails(int quantityToRelease)
    {
        // Arrange
        var product = Product.Create(id: 1, name: "product name", description: "product description", quantity: 10, price: 100m);
        product.Reserve(Guid.NewGuid(), 4);
        product.ClearDomainEvents();

        // Act
        var act = () => product.Release(Guid.NewGuid(), quantityToRelease);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Quantity must not be positive!");
    }
}
