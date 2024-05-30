using FluentResults;
using MediatR;
using Skimart.Application.Cases.Payment.Errors;
using Skimart.Application.Extensions.Transaction;
using Skimart.Application.Gateways.Payment;
using Skimart.Application.Gateways.Persistence.Repositories.StoreOrder;
using Skimart.Domain.Entities.Order;

namespace Skimart.Application.Cases.Payment.Commands.ConfirmPayment;

public class ConfirmPaymentHandler : IRequestHandler<ConfirmPaymentCommand, Result>
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly IOrderRepository _orderRepository;

    public ConfirmPaymentHandler(IPaymentGateway paymentGateway, IOrderRepository orderRepository)
    {
        _paymentGateway = paymentGateway;
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
            var paymentResult = _paymentGateway.ConfirmPayment(bodyContent, paymentEvent);
            var paymentIntent = paymentResult.PaymentIntent;

            if (paymentResult.IsSuccess)
            {
                await _orderRepository.UpdateOrderPayment(paymentIntent, OrderStatus.PaymentReceived);
            }
            else
            {
                await _orderRepository.UpdateOrderPayment(paymentIntent, OrderStatus.PaymentFailed);
            }

            var transactionResult = await _orderRepository.SaveChangesAsync();

            result = transactionResult.TransactionSuccess() ? Result.Ok() : Result.Fail("Could not update the order."); 
        }
        catch (Exception ex)
        {
            result = Result.Fail(ex.Message);
        }

        return result;
    }
}