using MediatR;

namespace Inventory.Application.Products.ReleaseStock;

public record ReleaseStockCommand(List<(int ProductId, int Quantity)> ItemsToRelease) : IRequest;
