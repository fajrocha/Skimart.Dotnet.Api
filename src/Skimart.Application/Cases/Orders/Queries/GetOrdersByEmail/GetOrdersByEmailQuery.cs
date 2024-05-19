using MediatR;
using Skimart.Application.Cases.Orders.Dtos;
using Skimart.Domain.Entities.Order;

namespace Skimart.Application.Cases.Orders.Queries.GetOrdersByEmail;

public record GetOrdersByEmailQuery(string Email) : IRequest<List<Order>>;