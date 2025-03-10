using MediatR;
using Order.Domain.Orders;

namespace Order.Application.Orders.AddLineItem;

internal class AddLineItemCommandHandler : IRequestHandler<AddLineItemCommand>
{
    private readonly IOrderRepository _orderRepository;

    public AddLineItemCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Handle(
        AddLineItemCommand command,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId)
            ?? throw new Exception("Order not found!");

        order.AddLineItem(command.ProductId, command.Quantity, command.Price);

        await _orderRepository.SaveChangesAsync();
    }
}
