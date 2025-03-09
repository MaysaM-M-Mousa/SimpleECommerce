using MediatR;
using Order.Domain.Orders;

namespace Order.Application.Orders.CancelOrder;

internal class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand>
{
    private readonly IOrderRepository _orderRepository;

    public CancelOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Handle(
        CancelOrderCommand command,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId)
            ?? throw new Exception("Order not found!");

        order.Cancel(command.Reason);

        await _orderRepository.SaveChangesAsync();
    }
}
