using Skimart.Application.Products.Gateways;
using Skimart.Application.Products.Queries.GetAllProducts;
using Skimart.Domain.Entities.Products;
using Skimart.Infrastructure.Products.Specifications;
using Skimart.Infrastructure.Shared;
using Skimart.Infrastructure.Shared.Repositories;
using Skimart.Infrastructure.Store;

namespace Skimart.Infrastructure.Products.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(StoreContext context) : base(context)
    {
    }

    public async Task<List<Product>> GetEntitiesAsync(GetAllProductsQuery productsQuery)
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