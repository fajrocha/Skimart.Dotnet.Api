using Skimart.Application.Orders.Gateways;
using Skimart.Domain.Entities.Order;
using Skimart.Infrastructure.Shared;
using Skimart.Infrastructure.Shared.Repositories;
using Skimart.Infrastructure.Store;

namespace Skimart.Infrastructure.Orders.Repositories;

public class DeliveryMethodRepository : BaseRepository<DeliveryMethod>, IDeliveryMethodRepository
{
    public DeliveryMethodRepository(StoreContext context) : base(context)
    {
    }
}