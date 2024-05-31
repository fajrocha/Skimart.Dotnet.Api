using Skimart.Application.Products.Gateways;
using Skimart.Domain.Entities.Products;
using Skimart.Infrastructure.Shared;
using Skimart.Infrastructure.Shared.Repositories;
using Skimart.Infrastructure.Store;

namespace Skimart.Infrastructure.Products.Repositories;

public class ProductBrandRepository : BaseRepository<ProductBrand>, IProductBrandRepository
{
    public ProductBrandRepository(StoreContext context) : base(context)
    {
    }
}