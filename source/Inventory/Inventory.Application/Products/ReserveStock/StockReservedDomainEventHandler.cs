using Inventory.Domain.Products.DomainEvents;
using Inventory.IntegrationEvents;
using MassTransit;
using MediatR;

namespace Inventory.Application.Products.ReserveStock;

internal class StockReservedDomainEventHandler : INotificationHandler<StockReservedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public StockReservedDomainEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(StockReservedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(new StockReservedIntegrationEvent
        {
            MessageId = notification.Id,
            ProductId = notification.ProductId,
            Quantity = notification.Quantity,
            OrderId = notification.OrderId,
            OccurredOnUtc = notification.OccurredOn
        });
    }
}
