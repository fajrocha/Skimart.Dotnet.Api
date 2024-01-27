using MediatR;
using Skimart.Application.Cases.Orders.Dtos;

namespace Skimart.Application.Cases.Orders.Queries.GetOrdersByEmail;

public record GetOrdersByEmailQuery(string Email) : IRequest<IReadOnlyList<OrderToReturnDto>>;