namespace Order.Api.DTOs;

public record CreateOrderRequest(
    string? Description, 
    Guid CustomerId);
