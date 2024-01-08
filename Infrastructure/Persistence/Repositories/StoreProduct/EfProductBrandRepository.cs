using Application.Abstractions.Persistence.Repositories.StoreProduct;
using Domain.Entities.Product;
using Infrastructure.Persistence.DbContexts;

namespace Infrastructure.Persistence.Repositories.StoreProduct;

public class EfProductBrandRepository : StoreRepository<ProductBrand>, IProductBrandRepository
{
    public EfProductBrandRepository(StoreContext context) : base(context)
    {
    }
}