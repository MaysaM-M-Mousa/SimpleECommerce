using MassTransit;
using MediatR;
using Order.Domain.Orders.Events;
using Order.IntegrationEvents;

namespace Order.Application.Orders.CancelOrder;

internal class OrderCancelledDomainEventHandler : INotificationHandler<OrderCancelledDomainEvent>
{
    private readonly IPublishEndpoint _publisher;

    public OrderCancelledDomainEventHandler(IPublishEndpoint publisher)
    {
        _publisher = publisher;
    }

    public async Task Handle(
        OrderCancelledDomainEvent notification, 
        CancellationToken cancellationToken)
    {
        var integrationEvent = new OrderCancelledIntegrationEvent
        {
            OrderId = notification.OrderId,
            MessageId = notification.Id,
            Reason = notification.Reason,
            IsPlaced = notification.IsPlaced,
            OccurredOnUtc = notification.OccurredOn,
            Items = notification.Items.Select(x => new IntegrationEvents.Item(x.ProductId, x.Quantity)).ToList(),
        };

        await _publisher.Publish(integrationEvent);
    }
}
