using MediatR;
using Skimart.Application.Gateways.Persistence.Repositories.StoreOrder;
using Skimart.Domain.Entities.Order;

namespace Skimart.Application.Cases.Orders.Queries.GetOrdersByEmail;

public class GetOrdersByEmailHandler : IRequestHandler<GetOrdersByEmailQuery, List<Order>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersByEmailHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task<List<Order>> Handle(GetOrdersByEmailQuery query, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetOrdersByEmailAsync(query.Email);
        
        return orders;
    }
}