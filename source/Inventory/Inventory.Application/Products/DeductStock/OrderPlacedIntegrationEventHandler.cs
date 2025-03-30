using MassTransit;
using MediatR;
using Order.IntegrationEvents;

namespace Inventory.Application.Products.DeductStock;

internal class OrderPlacedIntegrationEventHandler : IConsumer<OrderPlacedIntegrationEvent>
{
    private readonly IMediator _mediator;

    public OrderPlacedIntegrationEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<OrderPlacedIntegrationEvent> context)
    {
        var itemsToDeduct = context
            .Message
            .Items
            .Select(x => (x.ProductId, x.Quantity))
            .ToList();

        //await _mediator.Send(new DeductStockCommand(itemsToDeduct));
    }
}
