using Skimart.Application.Gateways.Persistence.Repositories.StoreProduct;
using Skimart.Domain.Entities.Products;
using Skimart.Infrastructure.Persistence.DbContexts;

namespace Skimart.Infrastructure.Persistence.Repositories.Products;

public class EfProductTypeRepository : BaseRepository<ProductType>, IProductTypeRepository
{
    public EfProductTypeRepository(StoreContext context) : base(context)
    {
    }
}