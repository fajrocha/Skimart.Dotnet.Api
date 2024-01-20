using Domain.Entities.Product;
using Skimart.Application.Abstractions.Persistence.Repositories.StoreProduct;
using Skimart.Application.Cases.Products.Queries.GetAllProducts;
using Skimart.Infrastructure.Persistence.DbContexts;
using Skimart.Infrastructure.Persistence.Specifications.ProductSpecifications;

namespace Skimart.Infrastructure.Persistence.Repositories.StoreProduct;

public class EfProductRepository : StoreRepository<Product>, IProductRepository
{
    public EfProductRepository(StoreContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Product>> GetEntitiesAsync(ProductParams productParams)
    {
        var spec = new ProductsWithTypesAndBrandsSpec(productParams);

        return await base.GetEntitiesAsync(spec);
    }

    public new async Task<Product?> GetEntityByIdAsync(int id)
    {
        var spec = new ProductByIdWithTypesAndBrandsSpec(id);
        
        return await base.GetEntityByIdAsync(spec);
    }

    public async Task<int> CountAsync(ProductParams productParams)
    {
        var spec = new ProductsWithFiltersCountSpec(productParams);

        return await base.CountAsync(spec);
    }
}