using Inventory.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.EntityConfigurations;

internal class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasData(new List<Product>()
        {
            Product.Create(id: 1, name: "IPhone 15", description: "The new branch IPhone from Apple!", quantity: 15, price: 1200),
            Product.Create(id: 2, name: "Asus ROG Strix G16", description: "High-performance gaming laptop from Asus", quantity: 150, price: 2000),
            Product.Create(id: 3, name: "Samsung TV", description: "Ultra FHD Samsung TV", quantity: 200, price: 3200),
            Product.Create(id: 4, name: "Sony WH-1000XM5", description: "Premium noise-canceling headphones", quantity: 40, price: 300),
            Product.Create(id: 5, name: "Amazon Echo Dot 5th Gen", description: "Compact and powerful smart speaker", quantity: 15, price: 150)
        });
    }
}
