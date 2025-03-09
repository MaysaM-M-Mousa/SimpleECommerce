using Inventory.Domain.Products;

namespace Inventory.Api.DTOs.Extensions;

public static class ProductsMappingExtensions
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto(product.Id, product.Name, product.Description, product.Quantity);
    }

    public static List<ProductDto> ToDto(this List<Product> products)
    {
        return products
            .Select(p => p.ToDto())
            .ToList();
    }
}
