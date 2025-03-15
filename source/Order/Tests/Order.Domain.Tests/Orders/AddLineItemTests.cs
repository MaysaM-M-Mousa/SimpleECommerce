using FluentAssertions;
using FluentAssertions.Execution;
using Order.Domain.Orders.Events;

namespace Order.Domain.Tests.Orders;

public class AddLineItemTests
{
    [Fact]
    public void AddingLineItem_ShouldAddNewLineItemRecord_WhenProductDoesNotExist()
    {
        // Arrange
        var order = Domain.Orders.Order.Create("Order Description", Guid.NewGuid());
        var productId = 1;
        var quantity = 2;
        var price = 10.0m;

        // Act
        order.AddLineItem(productId, quantity, price);

        // Assert
        using(new AssertionScope())
        {
            order.LineItems.Should().HaveCount(1);
            order.LineItems.First().ProductId.Should().Be(productId);
            order.LineItems.First().Quantity.Should().Be(quantity);
            order.TotalAmount.Should().Be(quantity * price);
            order.GetDomainEvents().Should().ContainSingle(e => e is LineItemAddedDomainEvent);
        }
    }

    [Fact]
    public void AddingLineItem_ShouldShouldIncreaseQuantity_WhenProductAlreadyExist()
    {
        // Arrange
        var order = Domain.Orders.Order.Create("Order Description", Guid.NewGuid());
        var productId = 1;
        var price = 10.0m;
        order.AddLineItem(productId, quantity: 2, price);
        order.ClearDomainEvents();

        // Act
        order.AddLineItem(productId, quantity: 3, price);

        // Assert
        using (new AssertionScope())
        {
            order.LineItems.Should().HaveCount(1);
            order.LineItems.First().Quantity.Should().Be(5);
            order.TotalAmount.Should().Be(5 * price);
        }
    }

    [Fact]
    public void AddingLineItemTo_PlacedOrder_Fails()
    {
        // Arrange
        var order = Domain.Orders.Order.Create("Order Description", Guid.NewGuid());
        order.AddLineItem(productId: 1, quantity: 2, price: 30);
        order.PlaceOrder();
        order.ClearDomainEvents();

        // Act
        var act = () => order.AddLineItem(productId: 2, quantity: 4, price: 40);

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Can't add items to placed order!");
    }

    [Fact]
    public void AddingLineItemTo_CancelledOrder_Fails()
    {
        // Arrange
        var order = Domain.Orders.Order.Create("Order Description", Guid.NewGuid());
        order.AddLineItem(productId: 1, quantity: 2, price: 30);
        order.Cancel("Help!, need some money!");
        order.ClearDomainEvents();

        // Act
        var act = () => order.AddLineItem(productId: 2, quantity: 4, price: 40);

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Can't add items to cancelled order!");
    }
}
