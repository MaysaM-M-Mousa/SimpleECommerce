namespace Inventory.Domain.Products;

public interface IProductRepository
{
    Task<List<Product>> GetAllProductsAsync();

    Task<Product?> GetByIdAsync(int id);

    Task SaveChangesAsync();
}
