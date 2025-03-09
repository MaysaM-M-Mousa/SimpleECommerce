using MediatR;

namespace Inventory.Application.Products.DeductStock;

public record DeductStockCommand(List<(int ProductId, int Quantity)> ItemsToDeduct) : IRequest;
