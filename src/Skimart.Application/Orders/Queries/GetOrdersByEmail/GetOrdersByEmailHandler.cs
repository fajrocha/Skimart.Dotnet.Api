using MediatR;
using Skimart.Application.Orders.Gateways;
using Skimart.Domain.Entities.Order;

namespace Skimart.Application.Orders.Queries.GetOrdersByEmail;

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