using Skimart.Domain.Entities.Order;
using Skimart.Infrastructure.Persistence.Specifications;

namespace Skimart.Infrastructure.Store.Specifications.OrderSpecifications;

public class OrderByPaymentIntentIdSpec : BaseSpecification<Order>
{
    public OrderByPaymentIntentIdSpec(string paymentIntentId) : base(o => o.PaymentIntentId == paymentIntentId)
    {
        AddInclude(o => o.OrderItems);
    }
}
