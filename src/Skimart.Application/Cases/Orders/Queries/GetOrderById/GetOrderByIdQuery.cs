using ErrorOr;
using MediatR;
using Skimart.Domain.Entities.Order;

namespace Skimart.Application.Cases.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(int Id) : IRequest<ErrorOr<Order>>;