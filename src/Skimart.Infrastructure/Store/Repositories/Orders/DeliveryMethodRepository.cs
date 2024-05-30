using Skimart.Application.Orders.Gateways;
using Skimart.Domain.Entities.Order;
using Skimart.Infrastructure.Store;

namespace Skimart.Infrastructure.Persistence.Repositories.Orders;

public class DeliveryMethodRepository : BaseRepository<DeliveryMethod>, IDeliveryMethodRepository
{
    public DeliveryMethodRepository(StoreContext context) : base(context)
    {
    }
}