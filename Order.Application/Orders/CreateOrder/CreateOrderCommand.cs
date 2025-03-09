using MediatR;

namespace Order.Application.Orders.CreateOrder;

public record CreateOrderCommand(
    string? Description,
    Guid customerId) : IRequest<Guid>;