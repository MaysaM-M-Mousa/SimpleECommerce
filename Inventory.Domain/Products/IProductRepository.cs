namespace Inventory.Domain.Products;

public interface IProductRepository
{
    Task<List<Product>> GetAllProductsAsync();

    Task<List<Product>> GetProductsByIdsAsync(List<int> productIds);

    Task<Product?> GetByIdAsync(int id);

    Task SaveChangesAsync();
}
