using MassTransit;
using MediatR;
using Order.Domain.Orders.Events;
using Order.IntegrationEvents;

namespace Order.Application.Orders.PlaceOrder;

internal class OrderPlacedDomainEventHandler : INotificationHandler<OrderPlacedDomainEvent>
{
    private readonly IPublishEndpoint _publisher;

    public OrderPlacedDomainEventHandler(IPublishEndpoint publisher)
    {
        _publisher = publisher;
    }

    public async Task Handle(
        OrderPlacedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var integrationEvent = new OrderPlacedIntegrationEvent()
        {
            OrderId = notification.OrderId,
            MessageId = notification.Id,
            CustomerId = notification.CustomerId,
            TotalAmount = notification.TotalAmount,
            OccurredOnUtc = notification.OccurredOn,
            Items = notification.Items,
        };

        await _publisher.Publish(integrationEvent);
    }
}
