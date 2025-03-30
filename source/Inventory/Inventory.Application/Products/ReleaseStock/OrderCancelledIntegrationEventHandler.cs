using MassTransit;
using MediatR;
using Order.IntegrationEvents;

namespace Inventory.Application.Products.ReleaseStock;

internal class OrderCancelledIntegrationEventHandler : IConsumer<OrderCancelledIntegrationEvent>
{
    private readonly IMediator _mediator;

    public OrderCancelledIntegrationEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<OrderCancelledIntegrationEvent> context)
    {
        if (!context.Message.IsPlaced)
        {
            return;
        }

        var itemsToRelease = context
                .Message
                .Items
                .Select(x => (x.ProductId, x.Quantity))
                .ToList();

        //await _mediator.Send(new ReleaseStockCommand(itemsToRelease));
    }
}
