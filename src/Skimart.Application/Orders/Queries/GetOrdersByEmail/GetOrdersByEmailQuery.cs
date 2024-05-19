using MediatR;
using Skimart.Domain.Entities.Order;

namespace Skimart.Application.Orders.Queries.GetOrdersByEmail;

public record GetOrdersByEmailQuery(string Email) : IRequest<List<Order>>;