using Skimart.Application.Shared.Gateways;
using Skimart.Domain.Entities.Order;

namespace Skimart.Application.Gateways.Persistence.Repositories.StoreOrder;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task<IReadOnlyList<Order>> GetOrdersByEmailAsync(string email);
    Task<Order?> GetOrdersByIdAndEmailAsync(int id, string email);
    Task<Order?> GetOrderByIntent(string paymentIntentId);
    Task<Order?> UpdateOrderPayment(string paymentIntentId, OrderStatus status);
}