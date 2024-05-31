using Skimart.Domain.Entities.Order;
using Skimart.Infrastructure.Shared.Specifications;

namespace Skimart.Infrastructure.Orders.Specifications;

public class OrderByPaymentIntentIdSpec : BaseSpecification<Order>
{
    public OrderByPaymentIntentIdSpec(string paymentIntentId) : base(o => o.PaymentIntentId == paymentIntentId)
    {
        AddInclude(o => o.OrderItems);
    }
}
