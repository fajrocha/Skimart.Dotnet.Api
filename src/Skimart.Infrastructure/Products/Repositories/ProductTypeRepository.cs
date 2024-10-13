using Skimart.Application.Products.Gateways;
using Skimart.Domain.Entities.Products;
using Skimart.Infrastructure.Shared;
using Skimart.Infrastructure.Shared.Repositories;
using Skimart.Infrastructure.Store;

namespace Skimart.Infrastructure.Products.Repositories;

public class ProductTypeRepository : BaseRepository<ProductType>, IProductTypeRepository
{
    public ProductTypeRepository(StoreContext context) : base(context)
    {
    }
}