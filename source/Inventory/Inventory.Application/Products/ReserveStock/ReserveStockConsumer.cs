using Inventory.Application.Products.ReserveStock.Saga;
using MassTransit;
using MediatR;

namespace Inventory.Application.Products.ReserveStock;

public class ReserveStockConsumer : IConsumer<ReserveStockRequest>
{
    private readonly IMediator _mediator;

    public ReserveStockConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ReserveStockRequest> context)
    {
        var request = context.Message;

        await _mediator.Send(new ReserveStockCommand(request.ProductId, request.Quantity, request.OrderId));
    }
}
