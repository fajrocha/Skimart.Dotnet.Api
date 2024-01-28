using FluentResults;
using Microsoft.Extensions.Primitives;
using Skimart.Application.Abstractions.Payment;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Infrastructure.Payment.Services;

public class StripePaymentService : IPaymentService
{
    public Task CreateOrUpdatePaymentIntentAsync(CustomerBasket basket, decimal shippingPrice)
    {
        throw new NotImplementedException();
    }

    public Result ConfirmPayment(string bodyContent, StringValues paymentEvent)
    {
        throw new NotImplementedException();
    }
}