using FluentResults;
using Microsoft.Extensions.Primitives;
using Skimart.Application.Cases.Payment.Commands.ConfirmPayment;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.Abstractions.Payment;

public interface IPaymentService
{
    Task CreateOrUpdatePaymentIntentAsync(CustomerBasket basket, decimal shippingPrice);

    PaymentResult ConfirmPayment(string bodyContent, StringValues paymentEvent);
}