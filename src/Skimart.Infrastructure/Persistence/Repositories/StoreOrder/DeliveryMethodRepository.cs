using Skimart.Application.Abstractions.Persistence.Repositories.StoreOrder;
using Skimart.Domain.Entities.Order;
using Skimart.Infrastructure.Persistence.DbContexts;

namespace Skimart.Infrastructure.Persistence.Repositories.StoreOrder;

public class DeliveryMethodRepository : BaseRepository<DeliveryMethod>, IDeliveryMethodRepository
{
    public DeliveryMethodRepository(StoreContext context) : base(context)
    {
    }
}