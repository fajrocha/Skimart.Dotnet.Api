using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Skimart.Application.Cases.Orders.Commands.CreateOrder;
using Skimart.Application.Cases.Orders.Dtos;
using Skimart.Application.Cases.Orders.Queries.GetDeliveryMethods;
using Skimart.Application.Cases.Orders.Queries.GetOrderById;
using Skimart.Application.Cases.Orders.Queries.GetOrdersByEmail;
using Skimart.Domain.Entities.Order;
using Skimart.Extensions.FluentResults;

namespace Skimart.Controllers;

public class OrdersController : BaseController
{
    private readonly IMediator _mediator;
    private const string ErrorMessage = "Error on Order request.";

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var command = new CreateOrderCommand(email, orderDto);

        var result = await _mediator.Send(command);

        return result.ToOkOrBadRequest(ErrorMessage);
    }
    
    [HttpGet]
    public async Task<IReadOnlyList<OrderToReturnDto>> GetOrderForUser()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var query = new GetOrdersByEmailQuery(email);

        return await _mediator.Send(query);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderToReturnDto>> GetOrderForUserById(int id)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var query = new GetOrderByIdQuery(id, email);

        var result = await _mediator.Send(query);

        return result.ToOkOrNotFound(ErrorMessage);
    }
    
    [HttpGet("deliveryMethods")]
    public async Task<IReadOnlyList<DeliveryMethod>> GetODeliveryMethods()
    {
        var query = new GetDeliveryMethodsQuery();

        return await _mediator.Send(query);
    }
}