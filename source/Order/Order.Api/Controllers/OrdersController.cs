using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Api.DTOs;
using Order.Application.Orders.AddLineItem;
using Order.Application.Orders.CancelOrder;
using Order.Application.Orders.CreateOrder;
using Order.Application.Orders.PlaceOrder;

namespace Order.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly ISender _sender;

    public OrdersController(ISender sender)
    {
        this._sender = sender;
    }

    [HttpPost]
    public async Task<object> CreateOrder([FromBody] CreateOrderRequest request)
    {
        return new
        {
            OrderId = await _sender.Send(new CreateOrderCommand(request.Description, request.CustomerId))
        };
    }

    [HttpPost("{orderId}/place")]
    public async Task PlaceOrder(Guid orderId)
    {
        await _sender.Send(new PlaceOrderCommand(orderId));
    }

    [HttpPost("{orderId}/items")]
    public async Task AddLineItem(Guid orderId, [FromBody] AddLineItemRequest request)
    {
        await _sender.Send(new AddLineItemCommand(orderId, request.ProductId, request.Quantity, request.Price));
    }

    [HttpPost("{orderId}/cancel")]
    public async Task CancelOrder(Guid orderId, [FromBody] CloseOrderRequest request)
    {
        await _sender.Send(new CancelOrderCommand(orderId, request.Reason));
    }
}
