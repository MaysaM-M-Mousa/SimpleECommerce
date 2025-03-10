using MediatR;

namespace Order.Application.Orders.PlaceOrder;

public record PlaceOrderCommand(Guid OrderId) : IRequest;