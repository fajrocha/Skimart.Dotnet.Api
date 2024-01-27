using FluentResults;
using MediatR;
using Skimart.Application.Cases.Orders.Dtos;

namespace Skimart.Application.Cases.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(int Id, string Email) : IRequest<Result<OrderToReturnDto>>;