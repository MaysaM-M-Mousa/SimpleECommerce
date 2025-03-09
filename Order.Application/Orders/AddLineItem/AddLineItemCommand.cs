using MediatR;

namespace Order.Application.Orders.AddLineItem;

public record AddLineItemCommand(
    Guid OrderId,
    int ProductId,
    int Quantity,
    decimal Price) : IRequest;
