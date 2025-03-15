using Inventory.Domain.Products;
using MediatR;

namespace Inventory.Application.Products.ReleaseStock;

internal class ReleaseStockCommandHandler : IRequestHandler<ReleaseStockCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IReleaseProductsDomainService _releaseProductsDomainService;

    public ReleaseStockCommandHandler(
        IProductRepository productRepository,
        IReleaseProductsDomainService releaseProductsDomainService)
    {
        _productRepository = productRepository;
        _releaseProductsDomainService = releaseProductsDomainService;
    }

    public async Task Handle(
        ReleaseStockCommand command, 
        CancellationToken cancellationToken)
    {
        var productIds = command
            .ItemsToRelease
            .Select(x => x.ProductId)
            .ToList();

        var products = await _productRepository.GetProductsByIdsAsync(productIds);

        _releaseProductsDomainService.ReleaseStocks(command.ItemsToRelease, products);

        await _productRepository.SaveChangesAsync();
    }
}
