using Inventory.Domain.Products;
using MediatR;

namespace Inventory.Application.Products.ReserveStock;

internal class ReserveStockCommandHandler : IRequestHandler<ReserveStockCommand>
{
    private readonly IProductRepository _productRepository;

    public ReserveStockCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(ReserveStockCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId);

        if (product is null)
        {
            throw new Exception("Product not found!");
        }

        product.Reserve(command.OrderId, command.Quantity);

        await _productRepository.SaveChangesAsync();
    }
}
