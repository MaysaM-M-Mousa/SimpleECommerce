using Inventory.Domain.Products;
using MediatR;

namespace Inventory.Application.Products.DeductStock;

internal class DeductStockCommandHandler : IRequestHandler<DeductStockCommand>
{
    private readonly IProductRepository _productRepository;

    public DeductStockCommandHandler(IProductRepository productRepository) 
    {
        _productRepository = productRepository;
    }

    public async Task Handle(
        DeductStockCommand command,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId);
        
        if (product is null)
        {
            throw new Exception("Product not found!");
        }

        product.Deduct(command.Quantity);

        await _productRepository.SaveChangesAsync();
    }
}
