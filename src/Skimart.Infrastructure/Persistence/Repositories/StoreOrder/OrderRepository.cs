using Skimart.Application.Abstractions.Persistence.Repositories.StoreOrder;
using Skimart.Domain.Entities.Order;
using Skimart.Infrastructure.Persistence.DbContexts;
using Skimart.Infrastructure.Persistence.Specifications.OrderSpecifications;

namespace Skimart.Infrastructure.Persistence.Repositories.StoreOrder;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(StoreContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Order>> GetOrdersByEmailAsync(string email)
    {
        var spec = new OrderWithItemsSpecification(email);

        return await GetEntitiesAsync(spec);
    }

    public async Task<Order?> GetOrdersByIdAndEmailAsync(int id, string email)
    {
        var spec = new OrderWithItemsSpecification(id, email);

        return await GetEntityByIdAsync(spec);
    }

    public async Task<Order?> GetOrderByIntent(string paymentIntentId)
    {
        var spec = new OrderByPaymentIntentIdSpec(paymentIntentId);
        
        return await GetEntityByIdAsync(spec);
    }

    public async Task<Order?> UpdateOrderPayment(string paymentIntentId, OrderStatus status)
    {
        var spec = new OrderByPaymentIntentIdSpec(paymentIntentId);

        var order = await GetEntityByIdAsync(spec);

        if (order is null) return order;

        order.Status = status;

        return order;
    }
}