using ErrorOr;
using FluentResults;
using MapsterMapper;
using MediatR;
using Skimart.Application.Gateways.Persistence.Repositories.StoreOrder;
using Skimart.Application.Identity.Gateways;
using Skimart.Domain.Entities.Order;
using Error = ErrorOr.Error;

namespace Skimart.Application.Cases.Orders.Queries.GetOrderById;

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, ErrorOr<Order>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IMapper _mapper;
    public GetOrderByIdHandler(IOrderRepository orderRepository, ICurrentUserProvider currentUserProvider)
    {
        _orderRepository = orderRepository;
        _currentUserProvider = currentUserProvider;
    }
    
    public async Task<ErrorOr<Order>> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
    {
        var user = _currentUserProvider.GetCurrentUserFromClaims();
        var orderId = query.Id;
        var order = await _orderRepository.GetOrdersByIdAndEmailAsync(orderId, user.Email);
        
        return order is not null ?
            order :
            Error.NotFound(description: "Order was not found.");
    }
}