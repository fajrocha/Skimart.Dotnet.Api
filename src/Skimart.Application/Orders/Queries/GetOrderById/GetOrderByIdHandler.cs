using ErrorOr;
using MapsterMapper;
using MediatR;
using Skimart.Application.Identity.Errors;
using Skimart.Application.Identity.Gateways;
using Skimart.Application.Orders.Errors;
using Skimart.Application.Orders.Gateways;
using Skimart.Domain.Entities.Order;
using Error = ErrorOr.Error;

namespace Skimart.Application.Orders.Queries.GetOrderById;

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, ErrorOr<Order>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICurrentUserProvider _currentUserProvider;
    public GetOrderByIdHandler(IOrderRepository orderRepository, ICurrentUserProvider currentUserProvider)
    {
        _orderRepository = orderRepository;
        _currentUserProvider = currentUserProvider;
    }
    
    public async Task<ErrorOr<Order>> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
    {
        var user = _currentUserProvider.GetCurrentUserFromClaims();

        if (user is null)
            return Error.NotFound(description: IdentityErrors.UserNotFoundOnToken);
        
        var orderId = query.Id;
        var order = await _orderRepository.GetOrdersByIdAndEmailAsync(orderId, user.Email);
        
        return order is not null ?
            order :
            Error.NotFound(description: OrderErrors.OrderNotFound);
    }
}