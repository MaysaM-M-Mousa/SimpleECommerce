using FluentAssertions;
using FluentAssertions.Execution;
using Order.Domain.Orders.Events;

namespace Order.Domain.Tests.Orders;

public class CreateTests
{
    [Fact]
    public void CreateNewOrder_Succeeds()
    {
        // Arrange
        string description = "Order 1 - description";
        Guid customerId = new Guid("70b4c215-9139-487d-8502-670953cd9107");

        // Act
        var order = Domain.Orders.Order.Create(description, customerId);

        // Assert
        using (new AssertionScope())
        {
            order.Should().NotBeNull();
            order.CustomerId.Should().Be(customerId);
            order.Description.Should().Be(description);
            order.GetDomainEvents().Count.Should().Be(1);
            order.GetDomainEvents().First().Should().BeOfType<OrderCreatedDomainEvent>();
        }
    }
}
