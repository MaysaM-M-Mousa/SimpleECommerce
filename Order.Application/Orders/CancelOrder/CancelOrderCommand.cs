using MediatR;

namespace Order.Application.Orders.CancelOrder;

public record CancelOrderCommand(
    Guid OrderId,
    string? Reason) : IRequest;
