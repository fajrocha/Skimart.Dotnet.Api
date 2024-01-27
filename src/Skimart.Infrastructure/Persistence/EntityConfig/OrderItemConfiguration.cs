using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skimart.Domain.Entities.Order;

namespace Skimart.Infrastructure.Persistence.EntityConfig;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.OwnsOne(i => i.ItemOrdered, poi => { poi.WithOwner(); });

        builder.Property(i => i.Price)
            .HasColumnType("decimal(18,2)");
    }
}