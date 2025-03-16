namespace Inventory.Domain.Products;

public class DeductProductsDomainService : IDeductProductsDomainService
{
    public void DeductStocks(IEnumerable<(int ProductId, int Quantity)> itemsToDeduct, List<Product> products)
    {
        if (itemsToDeduct.Count() != products.Count)
        {
            throw new Exception("No sufficient items!");
        }

        var productsById = products.ToDictionary(x => x.Id);

        foreach (var (productId, quantityToDeduct) in itemsToDeduct)
        {
            if (!productsById.TryGetValue(productId, out var existingProduct))
            {
                // Do nothing, return and don't proceed with this order placement
                throw new Exception("Could not find product!");
            }

            existingProduct.Deduct(quantityToDeduct);
        }

    }
}
