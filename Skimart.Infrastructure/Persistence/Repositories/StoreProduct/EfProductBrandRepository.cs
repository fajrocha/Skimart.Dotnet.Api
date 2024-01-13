using Domain.Entities.Product;
using Skimart.Application.Abstractions.Persistence.Repositories.StoreProduct;
using Skimart.Infrastructure.Persistence.DbContexts;

namespace Skimart.Infrastructure.Persistence.Repositories.StoreProduct;

public class EfProductBrandRepository : StoreRepository<ProductBrand>, IProductBrandRepository
{
    public EfProductBrandRepository(StoreContext context) : base(context)
    {
    }
}