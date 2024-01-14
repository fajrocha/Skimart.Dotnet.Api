using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skimart.Application.Cases.Basket.Commands.CreateOrUpdateBasket;
using Skimart.Application.Cases.Basket.Commands.DeleteBasket;
using Skimart.Application.Cases.Basket.Dtos;
using Skimart.Application.Cases.Basket.Queries.GetBasketById;
using Skimart.Application.Extensions.FluentResults;
using Skimart.Responses.ErrorResponses;

namespace Skimart.Controllers;

[AllowAnonymous]
public class BasketController : BaseController
{
    private readonly IMediator _mediator;
    private const string ErrorMessage = "Error on Basket request.";

    public BasketController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetBasketById(string id)
    {
        var query = new GetBasketByIdQuery(id);
        var result = await _mediator.Send(query);

        return result.IsSuccess ? 
            Ok(result.Value) : 
            NotFound(ErrorResponse.NotFound(result.GetReasonsAsCollection(), ErrorMessage));
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrUpdateBasket(CustomerBasketDto basket)
    {
        var command = new CreateOrUpdateBasketCommand(basket);
        var result = await _mediator.Send(command);
        
        return result.IsSuccess ?
            Ok(result.Value) : 
            BadRequest(ErrorResponse.BadRequest(result.GetReasonsAsCollection(), ErrorMessage));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteBasket(string id)
    {
        var command = new DeleteBasketCommand(id);
        var result = await _mediator.Send(command);

        return result ? Ok(result) : NotFound(result);
    }
    
    
}