using Skimart.Application.Gateways.Persistence.Repositories.StoreProduct;
using Skimart.Domain.Entities.Products;
using Skimart.Infrastructure.Persistence.DbContexts;

namespace Skimart.Infrastructure.Persistence.Repositories.Products;

public class EfProductBrandRepository : BaseRepository<ProductBrand>, IProductBrandRepository
{
    public EfProductBrandRepository(StoreContext context) : base(context)
    {
    }
}