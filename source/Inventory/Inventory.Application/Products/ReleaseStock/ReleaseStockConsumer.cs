using Inventory.Application.Products.ReserveStock.Saga;
using MassTransit;
using MediatR;

namespace Inventory.Application.Products.ReleaseStock;

public class ReleaseStockConsumer : IConsumer<ReleaseStockRequest>
{
    private readonly IMediator _mediator;

    public ReleaseStockConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ReleaseStockRequest> context)
    {
        var request = context.Message;

        await _mediator.Send(new ReleaseStockCommand(request.ProductId, request.Quantity, request.OrderId));
    }
}
