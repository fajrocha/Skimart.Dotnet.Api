using Microsoft.Extensions.Primitives;
using Skimart.Application.Cases.Payment.Commands.ConfirmPayment;
using Skimart.Application.Payment.Commands.ConfirmPayment;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.Gateways.Payment;

public interface IPaymentGateway
{
    Task CreateOrUpdatePaymentIntentAsync(CustomerBasket basket, decimal shippingPrice);

    PaymentResult ConfirmPayment(string bodyContent, StringValues paymentEvent);
}