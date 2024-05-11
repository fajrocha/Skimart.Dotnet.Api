using Skimart.Application.Gateways.Persistence.Repositories.StoreProduct;
using Skimart.Application.Products.Queries.GetAllProducts;
using Skimart.Domain.Entities.Products;
using Skimart.Infrastructure.Persistence.DbContexts;
using Skimart.Infrastructure.Persistence.Specifications.ProductSpecifications;

namespace Skimart.Infrastructure.Persistence.Repositories.Products;

public class EfProductRepository : BaseRepository<Product>, IProductRepository
{
    public EfProductRepository(StoreContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Product>> GetEntitiesAsync(GetAllProductsQuery productsQuery)
    {
        var spec = new ProductsWithTypesAndBrandsSpec(productsQuery);

        return await base.GetEntitiesAsync(spec);
    }

    public new async Task<Product?> GetEntityByIdAsync(int id)
    {
        var spec = new ProductByIdWithTypesAndBrandsSpec(id);
        
        return await base.GetEntityByIdAsync(spec);
    }

    public async Task<int> CountAsync(GetAllProductsQuery productQuery)
    {
        var spec = new ProductsWithFiltersCountSpec(productQuery);

        return await base.CountAsync(spec);
    }
}