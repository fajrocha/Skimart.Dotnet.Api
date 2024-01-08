using Application.Abstractions.Persistence.Repositories.StoreProduct;
using Domain.Entities.Product;
using Infrastructure.Persistence.DbContexts;

namespace Infrastructure.Persistence.Repositories.StoreProduct;

public class EfProductTypeRepository : StoreRepository<ProductType>, IProductTypeRepository
{
    public EfProductTypeRepository(StoreContext context) : base(context)
    {
    }
}