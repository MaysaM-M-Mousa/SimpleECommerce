using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Orders;

namespace Order.Persistence.Orders;

internal class LineItemEntityConfiguration : IEntityTypeConfiguration<LineItem>
{
    public void Configure(EntityTypeBuilder<LineItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasOne<Domain.Orders.Order>()
            .WithMany(x => x.LineItems)
            .HasForeignKey(x => x.OrderId)
            .IsRequired();
    }
}
