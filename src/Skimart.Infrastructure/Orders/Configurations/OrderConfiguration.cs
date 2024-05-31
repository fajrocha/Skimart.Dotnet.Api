using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skimart.Domain.Entities.Order;

namespace Skimart.Infrastructure.Orders.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsOne(o => o.ShippingAddress, a =>
        {
            a.WithOwner();
        });
        builder.Navigation(a => a.ShippingAddress).IsRequired();
        builder.Property(s => s.Status)
            .HasConversion(
                o => o.ToString(),
                o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o)
            );
        builder.Property(d => d.Subtotal).HasColumnType("decimal(18,2)");
        builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
    }
}