using FluentAssertions;
using Order.Domain.Orders;

namespace Order.Domain.Tests.LineItems;

public class IncreaseQuantityTests
{
    [Fact]
    public void Increasing_PositiveQuantity_Succeeds()
    {
        // Arrange
        var lineItem = LineItem.Create(id: Guid.NewGuid(), productId: 1, price: 50, quantity: 2, orderId: Guid.NewGuid());

        // Act
        lineItem.IncreaseQuantity(3);

        // Assert
        lineItem.Quantity.Should().Be(2 + 3);
    }

    [Fact]
    public void Increasing_NegativeQuantity_Fails()
    {
        // Arrange
        var lineItem = LineItem.Create(id: Guid.NewGuid(), productId: 1, price: 50, quantity: 2, orderId: Guid.NewGuid());

        // Act
        var act = () => lineItem.IncreaseQuantity(-2);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Quantity must be positive!");
    }
}
