namespace Inventory.Domain.Products;

public interface IReleaseProductsDomainService
{
    void ReleaseStocks(IEnumerable<(int ProductId, int Quantity)> itemsToRelease, List<Product> products);
}
