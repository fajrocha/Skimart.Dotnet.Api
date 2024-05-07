using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skimart.Application.Basket.Commands.DeleteBasket;
using Skimart.Application.Basket.Queries.GetBasketById;
using Skimart.Contracts.Basket.Requests;
using Skimart.Mappers.Basket;

namespace Skimart.Controllers;

[AllowAnonymous]
public class BasketController : BaseController
{
    private readonly IMediator _mediator;

    public BasketController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetBasketById(string id)
    {
        var query = new GetBasketByIdQuery(id);
        var result = await _mediator.Send(query);

        return result.Match(basket =>
            {
                var response = basket.ToResponse();
                return Ok(response);
            },
            Problem);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrUpdateBasket(CustomerBasketRequest customerBasketRequest)
    {
        var command = customerBasketRequest.ToCommand();
        var result = await _mediator.Send(command);
        
        return result.Match(
            resultantBasket =>
            {
                var response = resultantBasket.ToResponse();
                return Ok(response);
            },
            Problem);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteBasket(string id)
    {
        var command = new DeleteBasketCommand(id);
        var result = await _mediator.Send(command);

        return result.Match(
            _ => NoContent(), 
            Problem);
    }
}