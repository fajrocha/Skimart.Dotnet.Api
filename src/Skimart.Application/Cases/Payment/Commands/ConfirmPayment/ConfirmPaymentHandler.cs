using FluentResults;
using MediatR;
using Skimart.Application.Abstractions.Payment;
using Skimart.Application.Abstractions.Persistence.Repositories.StoreOrder;
using Skimart.Application.Cases.Payment.Errors;
using Skimart.Domain.Entities.Order;

namespace Skimart.Application.Cases.Payment.Commands.ConfirmPayment;

public class ConfirmPaymentHandler : IRequestHandler<ConfirmPaymentCommand, Result>
{
    private readonly IPaymentService _paymentService;
    private readonly IOrderRepository _orderRepository;

    public ConfirmPaymentHandler(IPaymentService paymentService, IOrderRepository orderRepository)
    {
        _paymentService = paymentService;
        _orderRepository = orderRepository;
    }
    
    public async Task<Result> Handle(ConfirmPaymentCommand command, CancellationToken cancellationToken)
    {
        var (bodyContent, paymentEvent) = command;
        
        if (bodyContent is null)
        {
            return Result.Fail(PaymentError.InvalidPaymentWebhookBody);
        }
        
        Result result;

        try
        {
            var paymentResult = _paymentService.ConfirmPayment(bodyContent, paymentEvent);
            var paymentIntent = paymentResult.Value;

            if (paymentResult.IsSuccess)
            {
                await _orderRepository.UpdateOrderPayment(paymentIntent, OrderStatus.PaymentReceived);
            }
            else
            {
                await _orderRepository.UpdateOrderPayment(paymentIntent, OrderStatus.PaymentReceived);
            }

            result = Result.Ok();
        }
        catch (Exception ex)
        {
            result = Result.Fail(ex.Message);
        }

        return result;
    }
}