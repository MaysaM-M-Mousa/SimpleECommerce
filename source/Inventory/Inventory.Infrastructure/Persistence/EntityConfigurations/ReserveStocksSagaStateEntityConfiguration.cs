using Inventory.Application.Products.ReserveStock.Saga;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.EntityConfigurations;

internal class ReserveStocksSagaStateEntityConfiguration : IEntityTypeConfiguration<ReserveStocksSagaState>
{
    public void Configure(EntityTypeBuilder<ReserveStocksSagaState> builder)
    {
        builder.HasKey(x => x.CorrelationId);

        builder.Property(x => x.CurrentState)
               .IsRequired();

        builder.Property(x => x.OrderId)
               .IsRequired();

        builder.OwnsOne(s => s.ReservationDetails, d =>
        {
            d.ToJson();
            d.OwnsMany(x => x.ProductsToReserve);
            d.OwnsMany(x => x.ReservedProducts);
            d.OwnsMany(x => x.ReleasedProducts);
        });
    }
}
