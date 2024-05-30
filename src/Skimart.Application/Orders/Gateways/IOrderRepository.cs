using Skimart.Application.Shared.Gateways;
using Skimart.Domain.Entities.Order;

namespace Skimart.Application.Orders.Gateways;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task<List<Order>> GetOrdersByEmailAsync(string email);
    Task<Order?> GetOrdersByIdAndEmailAsync(int id, string email);
    Task<Order?> GetOrderByIntent(string paymentIntentId);
    Task<Order?> UpdateOrderPayment(string paymentIntentId, OrderStatus status);
}