using Domain.Entities.Product;
using Skimart.Application.Abstractions.Persistence.Repositories.StoreProduct;
using Skimart.Infrastructure.Persistence.DbContexts;

namespace Skimart.Infrastructure.Persistence.Repositories.StoreProduct;

public class EfProductTypeRepository : StoreRepository<ProductType>, IProductTypeRepository
{
    public EfProductTypeRepository(StoreContext context) : base(context)
    {
    }
}