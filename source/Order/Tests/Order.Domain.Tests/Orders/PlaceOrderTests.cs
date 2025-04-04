using FluentAssertions;
using FluentAssertions.Execution;
using Order.Domain.Orders;
using Order.Domain.Orders.Events;

namespace Order.Domain.Tests.Orders;

public class PlaceOrderTests
{
    [Fact]
    public void PlaceOrder_ShouldChange_OrderStatus_To_Placed()
    {
        // Arrange
        var order = Domain.Orders.Order.Create("Order Description", Guid.NewGuid());
        order.AddLineItem(productId: 1, quantity: 2, price: 30);
        order.ClearDomainEvents();

        // Act
        order.PlaceOrder();

        // Assert
        using (new AssertionScope())
        {
            order.Status.Should().Be(OrderStatus.Placed);
            order.GetDomainEvents().Should().ContainSingle(e => e is OrderPlacedDomainEvent);
        }
    }

    [Fact]
    public void Placing_AlreadyPlacedOrder_Fails()
    {
        // Arrange
        var order = Domain.Orders.Order.Create("Order Description", Guid.NewGuid());
        order.AddLineItem(productId: 1, quantity: 2, price: 30);
        order.PlaceOrder();
        order.ClearDomainEvents();

        // Act
        var act = () => order.PlaceOrder();

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Order is already placed!");
    }

    [Fact]
    public void Placing_CancelledOrder_Fails()
    {
        // Arrange
        var order = Domain.Orders.Order.Create("Order Description", Guid.NewGuid());
        order.Cancel("No Budget!");
        order.ClearDomainEvents();

        // Act
        var act = () => order.PlaceOrder();

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Can't place a cancelled order!");
    }

    [Fact]
    public void Placing_EmptyOrder_Fails()
    {
        // Arrange
        var order = Domain.Orders.Order.Create("Order Description", Guid.NewGuid());
        order.ClearDomainEvents();

        // Act
        var act = () => order.PlaceOrder();

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Can't place an empty order!");
    }
}
