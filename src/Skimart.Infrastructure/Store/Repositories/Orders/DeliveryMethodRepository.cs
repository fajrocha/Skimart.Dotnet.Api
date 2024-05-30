using Skimart.Application.Gateways.Persistence.Repositories.StoreOrder;
using Skimart.Domain.Entities.Order;
using Skimart.Infrastructure.Store.DbContexts;

namespace Skimart.Infrastructure.Persistence.Repositories.Orders;

public class DeliveryMethodRepository : BaseRepository<DeliveryMethod>, IDeliveryMethodRepository
{
    public DeliveryMethodRepository(StoreContext context) : base(context)
    {
    }
}