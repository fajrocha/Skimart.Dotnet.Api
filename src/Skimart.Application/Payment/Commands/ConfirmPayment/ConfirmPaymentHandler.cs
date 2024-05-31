using ErrorOr;
using MediatR;
using Skimart.Application.Extensions.Transaction;
using Skimart.Application.Orders.Gateways;
using Skimart.Application.Payment.Errors;
using Skimart.Application.Payment.Gateways;
using Skimart.Domain.Entities.Order;

namespace Skimart.Application.Payment.Commands.ConfirmPayment;

public class ConfirmPaymentHandler : IRequestHandler<ConfirmPaymentCommand, ErrorOr<Success>>
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly IOrderRepository _orderRepository;

    public ConfirmPaymentHandler(IPaymentGateway paymentGateway, IOrderRepository orderRepository)
    {
        _paymentGateway = paymentGateway;
        _orderRepository = orderRepository;
    }
    
    public async Task<ErrorOr<Success>> Handle(ConfirmPaymentCommand command, CancellationToken cancellationToken)
    {
        var (bodyContent, paymentEvent) = command;
        
        if (bodyContent is null)
        {
            return Error.Failure(description: PaymentError.InvalidPaymentWebhookBody);
        }
        
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

            return transactionResult.TransactionSuccess() ? 
                Result.Success : 
                Error.Failure(description: PaymentError.FailedToUpdateOrder); 
        }
        catch (Exception ex)
        {
            return Error.Failure(description: ex.Message);
        }
    }
}