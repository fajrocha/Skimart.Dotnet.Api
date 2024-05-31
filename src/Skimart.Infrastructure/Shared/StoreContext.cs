using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Skimart.Application.Shared.Gateways;
using Skimart.Domain.Entities.Order;
using Skimart.Domain.Entities.Products;

namespace Skimart.Infrastructure.Shared;

public class StoreContext : DbContext, IUnitOfWork
{
    public StoreContext(DbContextOptions<StoreContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductBrand> ProductBrands { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
    
    public Task<int> CommitAsync()
    {
        return base.SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}