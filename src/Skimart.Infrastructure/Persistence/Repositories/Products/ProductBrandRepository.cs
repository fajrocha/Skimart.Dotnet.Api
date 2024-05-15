using Skimart.Application.Products.Gateways;
using Skimart.Domain.Entities.Products;
using Skimart.Infrastructure.Persistence.DbContexts;

namespace Skimart.Infrastructure.Persistence.Repositories.Products;

public class ProductBrandRepository : BaseRepository<ProductBrand>, IProductBrandRepository
{
    public ProductBrandRepository(StoreContext context) : base(context)
    {
    }
}