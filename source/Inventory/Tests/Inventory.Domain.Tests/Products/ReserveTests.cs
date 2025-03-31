using FluentAssertions;
using FluentAssertions.Execution;
using Inventory.Domain.Products;
using Inventory.Domain.Products.DomainEvents;

namespace Inventory.Domain.Tests.Products;

public class ReserveTests
{
    [Fact]
    public void Reserving_PositiveQuantity_AddsOrderReservations()
    {
        // Arrange
        var product = Product.Create(id: 1, name: "product name", description: "product description", quantity: 10, price: 100m);
        var orderId = Guid.NewGuid();
        product.ClearDomainEvents();

        // Act
        product.Reserve(orderId, 2);

        // Assert
        using (new AssertionScope())
        {
            product.ReservedQuantity.Should().Be(2);
            product.AvailableQuantity.Should().Be(10 - 2);
            product.Reservations.Should().ContainSingle(r => r.OrderId == orderId);
            product.GetDomainEvents().Should().ContainSingle(e => e is StockReservedDomainEvent);
        }
    }

    [Fact]
    public void Reserving_ForAlreadyReservedOrder_Fails()
    {
        // Arrange
        var product = Product.Create(id: 1, name: "product name", description: "product description", quantity: 10, price: 100m);
        var orderId = Guid.NewGuid();
        product.Reserve(orderId, 2);
        product.ClearDomainEvents();

        // Act
        var act = () => product.Reserve(orderId, 4);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Order already has a reservation!");
    }

    [Fact]
    public void Reserving_WhenNoEnoughAvailableStocks_Fails()
    {
        // Arrange
        var product = Product.Create(id: 1, name: "product name", description: "product description", quantity: 10, price: 100m);
        var (orderId, secondOrderId) = (Guid.NewGuid(), Guid.NewGuid());
        product.Reserve(orderId, 8);
        product.ClearDomainEvents();

        // Act
        var act = () => product.Reserve(secondOrderId, 4);

        // Assert
        act.Should().Throw<Exception>().WithMessage("No enough available stocks to reserve!");
    }
}
