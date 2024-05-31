using Microsoft.Extensions.Primitives;
using Skimart.Application.Payment.Commands.ConfirmPayment;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.Payment.Gateways;

public interface IPaymentGateway
{
    Task CreateOrUpdatePaymentIntentAsync(CustomerBasket basket, decimal shippingPrice);

    PaymentResult ConfirmPayment(string bodyContent, StringValues paymentEvent);
}