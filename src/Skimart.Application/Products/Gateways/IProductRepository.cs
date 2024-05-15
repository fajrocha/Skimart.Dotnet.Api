using Skimart.Application.Products.Queries.GetAllProducts;
using Skimart.Application.Shared.Gateways;
using Skimart.Domain.Entities.Products;

namespace Skimart.Application.Products.Gateways;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<List<Product>> GetEntitiesAsync(GetAllProductsQuery productsQuery);
    Task<int> CountAsync(GetAllProductsQuery productQuery);
}