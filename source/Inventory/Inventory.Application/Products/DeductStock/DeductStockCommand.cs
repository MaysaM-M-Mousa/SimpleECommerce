using MediatR;

namespace Inventory.Application.Products.DeductStock;

public record DeductStockCommand(int ProductId, int Quantity) : IRequest;
