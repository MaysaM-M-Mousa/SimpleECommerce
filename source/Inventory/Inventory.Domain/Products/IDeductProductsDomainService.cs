namespace Inventory.Domain.Products;

public interface IDeductProductsDomainService
{
    void DeductStocks(IEnumerable<(int ProductId, int Quantity)> itemsToDeduct, List<Product> products);
}
