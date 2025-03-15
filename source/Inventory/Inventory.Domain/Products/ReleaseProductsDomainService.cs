namespace Inventory.Domain.Products;

public class ReleaseProductsDomainService : IReleaseProductsDomainService
{
    public void ReleaseStocks(
        IEnumerable<(int ProductId, int Quantity)> itemsToRelease,
        List<Product> products)
    {
        if (itemsToRelease.Count() != products.Count)
        {
            throw new Exception("Missing items!");
        }

        var productsById = products.ToDictionary(x => x.Id);

        foreach (var (productId, quantityToRelease) in itemsToRelease)
        {
            if (!productsById.TryGetValue(productId, out var existingProduct))
            {
                throw new Exception("Some products are not present!");
            }

            existingProduct.Release(quantityToRelease);
        }
    }
}
