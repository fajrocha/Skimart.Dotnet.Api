using Skimart.Domain.Entities.Order;

namespace Skimart.Infrastructure.Persistence.Specifications.OrderSpecifications;

public class OrderByPaymentIntentIdSpec : BaseSpecification<Order>
{
    public OrderByPaymentIntentIdSpec(string paymentIntentId) : base(o => o.PaymentIntentId == paymentIntentId)
    {
    }
}
