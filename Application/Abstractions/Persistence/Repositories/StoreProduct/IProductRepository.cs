using Application.Cases.Products.Queries.GetAllProducts;
using Domain.Entities.Product;

namespace Application.Abstractions.Persistence.Repositories.StoreProduct;

public interface IProductRepository : IStoreRepository<Product>
{
    Task<IReadOnlyList<Product>> GetEntitiesAsync(ProductParams productParams);
    new Task<Product?> GetEntityByIdAsync(int id);
    Task<int> CountAsync(ProductParams productParams);
}