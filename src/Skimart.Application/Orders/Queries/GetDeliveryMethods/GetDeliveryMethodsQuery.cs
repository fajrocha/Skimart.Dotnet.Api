using MediatR;
using Skimart.Domain.Entities.Order;

namespace Skimart.Application.Orders.Queries.GetDeliveryMethods;

public record GetDeliveryMethodsQuery() : IRequest<List<DeliveryMethod>>;