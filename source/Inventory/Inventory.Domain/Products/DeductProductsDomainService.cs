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
            if (!productsById.TryGetValue(productId, out var existingProduct) ||
                !existingProduct.CanDeduct(quantityToDeduct))
            {
                // Do nothing, return and don't proceed with this order placement
                throw new Exception("Some products are out of stock!");
            }

            existingProduct.Deduct(quantityToDeduct);
        }

        // append domain event as outbox msg here so it's wrapped in the same transaction

    }
}
