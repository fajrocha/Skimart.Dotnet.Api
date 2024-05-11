using Skimart.Application.Products.Queries.GetAllProducts;
using Skimart.Domain.Entities.Products;

namespace Skimart.Application.Gateways.Persistence.Repositories.StoreProduct;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<IReadOnlyList<Product>> GetEntitiesAsync(GetAllProductsQuery productsQuery);
    Task<int> CountAsync(GetAllProductsQuery productQuery);
}