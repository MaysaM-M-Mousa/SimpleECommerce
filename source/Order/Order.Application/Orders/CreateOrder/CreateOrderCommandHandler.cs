using MediatR;
using Order.Domain.Orders;
using OrderAgg = Order.Domain.Orders.Order;

namespace Order.Application.Orders.CreateOrder;

internal class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Guid> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = OrderAgg.Create(request.Description, request.customerId);

        _orderRepository.AddOrder(order);

        await _orderRepository.SaveChangesAsync();

        return order.Id;
    }
}

