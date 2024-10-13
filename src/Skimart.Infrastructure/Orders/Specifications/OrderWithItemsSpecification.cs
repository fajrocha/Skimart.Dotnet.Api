using Skimart.Domain.Entities.Order;
using Skimart.Infrastructure.Shared.Specifications;

namespace Skimart.Infrastructure.Orders.Specifications;

public class OrderWithItemsSpecification : BaseSpecification<Order>
{
    public OrderWithItemsSpecification(string email) 
        : base(o => o.BuyerEmail == email )
    {
        IncludeOrderItemsAndDelivery();
        AddOrderByDescending(o => o.OrderDate);
    }
    
    public OrderWithItemsSpecification(int id, string email) 
        : base(o => o.Id == id && o.BuyerEmail == email )
    {
        IncludeOrderItemsAndDelivery();
        AddOrderByDescending(o => o.OrderDate);
    }
    
    private void IncludeOrderItemsAndDelivery()
    {
        AddInclude(o => o.OrderItems);
        AddInclude(o => o.DeliveryMethod);
    }
}