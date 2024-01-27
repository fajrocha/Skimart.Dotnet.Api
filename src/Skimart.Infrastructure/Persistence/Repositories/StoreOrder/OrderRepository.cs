using Skimart.Application.Abstractions.Persistence.Repositories.StoreOrder;
using Skimart.Domain.Entities.Order;
using Skimart.Infrastructure.Persistence.DbContexts;

namespace Skimart.Infrastructure.Persistence.Repositories.StoreOrder;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(StoreContext context) : base(context)
    {
    }

    public Task<IReadOnlyList<Order>> GetOrdersOrderedDescAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<Order?> GetOrdersByIdAsync(int id, string email)
    {
        throw new NotImplementedException();
    }

    public Task<Order?> GetOrderByIntent(string paymentIntentId)
    {
        throw new NotImplementedException();
    }

    public Task<Order?> UpdateOrderPayment(string paymentIntentId, OrderStatus status)
    {
        throw new NotImplementedException();
    }
}