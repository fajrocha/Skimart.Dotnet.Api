using MediatR;
using Skimart.Domain.Entities.Order;

namespace Skimart.Application.Cases.Orders.Queries.GetDeliveryMethods;

public record GetDeliveryMethodsQuery() : IRequest<List<DeliveryMethod>>;