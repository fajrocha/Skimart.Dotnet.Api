using MediatR;
using Microsoft.AspNetCore.Mvc;
using Skimart.Application.Cases.Payment.Commands.ConfirmPayment;
using Skimart.Application.Cases.Payment.Commands.CreateOrUpdatePaymentIntent;
using Skimart.Application.Extensions.FluentResults;
using Skimart.Domain.Entities.Basket;
using Skimart.Extensions.FluentResults;

namespace Skimart.Controllers;

public class PaymentController : BaseController
{
    private readonly IMediator _mediator;
    private const string ErrorMessage = "Error on Payment request.";
    private const string EventHeader = "Stripe-Signature";

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
    
    [HttpPost("webhook")]
    public async Task<ActionResult> PaymentServiceWebhook()
    {
        var bodyContent = await new StreamReader(Request.Body).ReadToEndAsync();
        var paymentEvent = Request.Headers[EventHeader];
        var command = new ConfirmPaymentCommand(bodyContent, paymentEvent);

        var result = await _mediator.Send(command);

        return result.ToOkOrBadRequest(ErrorMessage);
    }
}