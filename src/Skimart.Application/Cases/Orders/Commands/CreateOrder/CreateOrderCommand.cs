using FluentResults;
using MediatR;
using Skimart.Application.Cases.Orders.Dtos;

namespace Skimart.Application.Cases.Orders.Commands.CreateOrder;

public record CreateOrderCommand(string BuyerEmail, OrderDto OrderDto) : IRequest<Result<OrderToReturnDto>>;