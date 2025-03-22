using FluentAssertions;
using Order.Domain.Orders;

namespace Order.Domain.Tests.LineItems;

public class CalculateTotalPriceTests
{
    [Fact]
    public void CalculateTotalPrice_ReturnsCorrectValue()
    {
        // Arrange
        var lineItem = LineItem.Create(id: Guid.NewGuid(), productId: 1, price: 50.5m, quantity: 2, orderId: Guid.NewGuid());

        // Act
        var totalPrice = lineItem.CalculateTotalPrice();

        // Assert
        totalPrice.Should().Be(50.5m * 2);
    }
}
