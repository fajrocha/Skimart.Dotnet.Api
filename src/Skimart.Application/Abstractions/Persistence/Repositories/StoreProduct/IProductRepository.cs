using Domain.Entities.Product;
using Skimart.Application.Cases.Products.Queries.GetAllProducts;

namespace Skimart.Application.Abstractions.Persistence.Repositories.StoreProduct;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<IReadOnlyList<Product>> GetEntitiesAsync(ProductParams productParams);
    new Task<Product?> GetEntityByIdAsync(int id);
    Task<int> CountAsync(ProductParams productParams);
}