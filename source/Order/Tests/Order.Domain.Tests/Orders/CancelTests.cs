using FluentAssertions.Execution;
using FluentAssertions;
using Order.Domain.Orders;
using Order.Domain.Orders.Events;

namespace Order.Domain.Tests.Orders;

public class CancelTests
{
    [Fact]
    public void CancellingOrder_ShouldChange_Status_ToCancelled()
    {
        // Arrange
        var order = Domain.Orders.Order.Create("Order Description", Guid.NewGuid());
        order.AddLineItem(productId: 1, quantity: 2, price: 30);
        order.ClearDomainEvents();

        // Act
        order.Cancel("No Budget!");

        // Assert
        using (new AssertionScope())
        {
            order.Status.Should().Be(OrderStatus.Cancel);
            order.GetDomainEvents().Should().ContainSingle(e => e is OrderCancelledDomainEvent);
        }
    }

    [Fact]
    public void Cancelling_AlreadyCancelledOrder_Fails()
    {
        // Arrange
        var order = Domain.Orders.Order.Create("Order Description", Guid.NewGuid());
        order.AddLineItem(productId: 1, quantity: 2, price: 30);
        order.Cancel("No Budget!");
        order.ClearDomainEvents();

        // Act
        var act = () => order.Cancel("In debt ;(");

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Already Cancelled!");    
    }
}
