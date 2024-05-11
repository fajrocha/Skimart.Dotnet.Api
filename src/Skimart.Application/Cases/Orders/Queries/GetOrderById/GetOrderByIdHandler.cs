using FluentResults;
using MapsterMapper;
using MediatR;
using Skimart.Application.Cases.Orders.Dtos;
using Skimart.Application.Cases.Orders.Errors;
using Skimart.Application.Gateways.Persistence.Repositories.StoreOrder;

namespace Skimart.Application.Cases.Orders.Queries.GetOrderById;

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, Result<OrderToReturnDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrderByIdHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }
    
    public async Task<Result<OrderToReturnDto>> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
    {
        var orderId = query.Id;
        var order = await _orderRepository.GetOrdersByIdAndEmailAsync(orderId, query.Email);
        
        return order is null ?
            Result.Fail(OrderError.OrderNotFound(orderId)) :
            Result.Ok(_mapper.Map<OrderToReturnDto>(order));
    }
}