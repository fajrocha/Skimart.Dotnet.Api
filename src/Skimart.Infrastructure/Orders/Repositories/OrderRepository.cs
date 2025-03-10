using Skimart.Application.Orders.Gateways;
using Skimart.Domain.Entities.Order;
using Skimart.Infrastructure.Orders.Specifications;
using Skimart.Infrastructure.Shared;
using Skimart.Infrastructure.Shared.Repositories;
using Skimart.Infrastructure.Store;

namespace Skimart.Infrastructure.Orders.Repositories;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(StoreContext context) : base(context)
    {
    }

    public async Task<List<Order>> GetOrdersByEmailAsync(string email)
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