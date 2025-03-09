using Inventory.Domain.Products;
using MediatR;

namespace Inventory.Application.Products.GetAllProducts;

public record GetAllProductsQuery : IRequest<List<Product>>;

internal class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<Product>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        return await _productRepository.GetAllProductsAsync();
    }
}
