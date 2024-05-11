using MapsterMapper;
using MediatR;
using Skimart.Application.Cases.Orders.Dtos;
using Skimart.Application.Gateways.Persistence.Repositories.StoreOrder;

namespace Skimart.Application.Cases.Orders.Queries.GetOrdersByEmail;

public class GetOrdersByEmailHandler : IRequestHandler<GetOrdersByEmailQuery, IReadOnlyList<OrderToReturnDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrdersByEmailHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }
    
    public async Task<IReadOnlyList<OrderToReturnDto>> Handle(GetOrdersByEmailQuery query, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetOrdersByEmailAsync(query.Email);
        
        return _mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);
    }
}