using MediatR;

namespace Inventory.Application.Products.ReleaseStock;

public record ReleaseStockCommand(int ProductId, int Quantity, Guid OrderId) : IRequest;
