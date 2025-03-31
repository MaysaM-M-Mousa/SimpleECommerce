using MediatR;

namespace Inventory.Application.Products.ReserveStock;

public record ReserveStockCommand(int ProductId, int Quantity, Guid OrderId) : IRequest;
