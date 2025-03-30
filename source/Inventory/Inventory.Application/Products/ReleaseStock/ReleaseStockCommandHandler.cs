using Inventory.Domain.Products;
using MediatR;

namespace Inventory.Application.Products.ReleaseStock;

internal class ReleaseStockCommandHandler : IRequestHandler<ReleaseStockCommand>
{
    private readonly IProductRepository _productRepository;
    public ReleaseStockCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(
        ReleaseStockCommand command, 
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId);

        if (product is null)
        {
            throw new Exception("Product not found!");
        }

        product.Release(command.Quantity);

        await _productRepository.SaveChangesAsync();
    }
}
