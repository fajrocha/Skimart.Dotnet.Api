using Skimart.Application.Products.Gateways;
using Skimart.Domain.Entities.Products;
using Skimart.Infrastructure.Store.DbContexts;

namespace Skimart.Infrastructure.Persistence.Repositories.Products;

public class ProductTypeRepository : BaseRepository<ProductType>, IProductTypeRepository
{
    public ProductTypeRepository(StoreContext context) : base(context)
    {
    }
}