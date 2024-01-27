using Skimart.Domain.Entities.Order;

namespace Skimart.Application.Abstractions.Persistence.Repositories.StoreOrder;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task<IReadOnlyList<Order>> GetOrdersOrderedDescAsync(string email);
    Task<Order?> GetOrdersByIdAsync(int id, string email);
    Task<Order?> GetOrderByIntent(string paymentIntentId);
    Task<Order?> UpdateOrderPayment(string paymentIntentId, OrderStatus status);
}