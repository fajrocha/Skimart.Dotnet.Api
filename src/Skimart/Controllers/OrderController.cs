using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Skimart.Application.Orders.Queries.GetDeliveryMethods;
using Skimart.Application.Orders.Queries.GetOrderById;
using Skimart.Application.Orders.Queries.GetOrdersByEmail;
using Skimart.Contracts.Orders.Requests;
using Skimart.Mappers.Orders;

namespace Skimart.Controllers;

public class OrdersController : BaseController
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderRequest orderRequest)
    {
        var command = orderRequest.ToCommand();

        var result = await _mediator.Send(command);

        return result.Match(
            order => Ok(order.ToResponse()),
                Problem);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetOrderForUser()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var query = new GetOrdersByEmailQuery(email);
        var orders = await _mediator.Send(query);

        return Ok(orders.ToResponse());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderForUserById(int id)
    {
        var query = new GetOrderByIdQuery(id);

        var result = await _mediator.Send(query);

        return result.Match(
            order => Ok(order.ToResponse()),
            Problem);
    }
    
    [HttpGet("deliveryMethods")]
    public async Task<IActionResult> GetODeliveryMethods()
    {
        var query = new GetDeliveryMethodsQuery();

        var deliveryMethods = await _mediator.Send(query);
        
        return Ok(deliveryMethods.ToResponse());
    }
}