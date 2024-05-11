using MediatR;
using Skimart.Application.Gateways.Persistence.Repositories.StoreOrder;
using Skimart.Domain.Entities.Order;

namespace Skimart.Application.Cases.Orders.Queries.GetDeliveryMethods;

public class GetDeliveryMethodsHandler : IRequestHandler<GetDeliveryMethodsQuery, IReadOnlyList<DeliveryMethod>>
{
    private readonly IDeliveryMethodRepository _deliveryMethodRepository;

    public GetDeliveryMethodsHandler(IDeliveryMethodRepository deliveryMethodRepository)
    {
        _deliveryMethodRepository = deliveryMethodRepository;
    }


    public async Task<IReadOnlyList<DeliveryMethod>> Handle(GetDeliveryMethodsQuery request, CancellationToken cancellationToken)
    {
        return await _deliveryMethodRepository.GetEntitiesAsync();
    }
}