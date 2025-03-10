namespace Order.Api.DTOs;

public record AddLineItemRequest(
    int ProductId,
    int Quantity,
    decimal Price);
