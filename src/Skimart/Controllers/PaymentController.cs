using MediatR;
using Microsoft.AspNetCore.Mvc;
using Skimart.Application.Cases.Payment.Commands.CreateOrUpdatePaymentIntent;
using Skimart.Application.Extensions.FluentResults;
using Skimart.Domain.Entities.Basket;
using Skimart.Extensions.FluentResults;
using Skimart.Responses.ErrorResponses;

namespace Skimart.Controllers;

public class PaymentController : BaseController
{
    private readonly IMediator _mediator;
    private const string ErrorMessage = "Error on Payment request.";

    public PaymentController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("{basketId}")]
    public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
    {
        var command = new CreateOrUpdatePaymentIntentCommand(basketId);

        var result = await _mediator.Send(command);
        
        return result.ToOkOrBadRequest(ErrorMessage);
    }
}