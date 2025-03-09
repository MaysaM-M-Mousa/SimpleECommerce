using MediatR;
using Order.Domain.Orders;

namespace Order.Application.Orders.PlaceOrder;

internal class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand>
{
    private readonly IOrderRepository _orderRepository;

    public PlaceOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Handle(
        PlaceOrderCommand command,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId)
            ?? throw new Exception("Order not found!");

        order.PlaceOrder();

        await _orderRepository.SaveChangesAsync(cancellationToken);
    }
}
