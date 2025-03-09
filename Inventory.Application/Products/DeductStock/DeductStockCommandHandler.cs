using Inventory.Domain.Products;
using MediatR;

namespace Inventory.Application.Products.DeductStock;

internal class DeductStockCommandHandler : IRequestHandler<DeductStockCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IDeductProductsDomainService _deductProductsDomainService;

    public DeductStockCommandHandler(
        IProductRepository productRepository, 
        IDeductProductsDomainService deductProductsDomainService)
    {
        _productRepository = productRepository;
        _deductProductsDomainService = deductProductsDomainService;
    }

    public async Task Handle(
        DeductStockCommand command,
        CancellationToken cancellationToken)
    {
        var productIds = command
            .ItemsToDeduct
            .Select(x => x.ProductId)
            .ToList();

        var products = await _productRepository.GetProductsByIdsAsync(productIds);

        if (products.Count == 0)
        {
            return;
        }

        _deductProductsDomainService.DeductStocks(command.ItemsToDeduct, products);

        await _productRepository.SaveChangesAsync();
    }
}
