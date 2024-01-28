using FluentResults;
using Microsoft.Extensions.Primitives;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.Abstractions.Payment;

public interface IPaymentService
{
    Task CreateOrUpdatePaymentIntentAsync(CustomerBasket basket, decimal shippingPrice);

    Result ConfirmPayment(string bodyContent, StringValues paymentEvent);
}