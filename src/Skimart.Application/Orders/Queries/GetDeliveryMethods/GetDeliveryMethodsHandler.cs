using MediatR;
using Skimart.Application.Gateways.Persistence.Repositories.StoreOrder;
using Skimart.Domain.Entities.Order;

namespace Skimart.Application.Orders.Queries.GetDeliveryMethods;

public class GetDeliveryMethodsHandler : IRequestHandler<GetDeliveryMethodsQuery, List<DeliveryMethod>>
{
    private readonly IDeliveryMethodRepository _deliveryMethodRepository;

    public GetDeliveryMethodsHandler(IDeliveryMethodRepository deliveryMethodRepository)
    {
        _deliveryMethodRepository = deliveryMethodRepository;
    }

    public async Task<List<DeliveryMethod>> Handle(GetDeliveryMethodsQuery request, CancellationToken cancellationToken)
    {
        return await _deliveryMethodRepository.GetEntitiesAsync();
    }
}