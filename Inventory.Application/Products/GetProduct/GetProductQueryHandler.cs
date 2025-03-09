using Inventory.Domain.Products;
using MediatR;

namespace Inventory.Application.Products.GetProduct;

internal class GetProductQueryHandler : IRequestHandler<GetProductQuery, Product?>
{
    private readonly IProductRepository _productRepository;

    public GetProductQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product?> Handle(
        GetProductQuery query,
        CancellationToken cancellationToken)
    {
        return await _productRepository.GetByIdAsync(query.ProductId);
    }
}
