using Inventory.Domain.Products.DomainEvents;
using Inventory.IntegrationEvents;
using MassTransit;
using MediatR;

namespace Inventory.Application.Products.ReleaseStock;

internal class StockReleasedDomainEventHandler : INotificationHandler<StockReleasedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public StockReleasedDomainEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(StockReleasedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(new StockReleasedIntegrationEvent
        {
            MessageId = notification.Id,
            ProductId = notification.ProductId,
            Quantity = notification.Quantity,
            OrderId = notification.OrderId,
            OccurredOnUtc = notification.OccurredOn
        });
    }
}
