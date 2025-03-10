namespace Inventory.Api.DTOs;

public record ProductDto(
    int ProductId,
    string Name,
    string Description,
    int Quantity);
