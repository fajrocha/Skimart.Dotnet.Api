using FluentResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Skimart.Application.Cases.Payment.Commands.ConfirmPayment;
using Skimart.Application.Configurations.Payment;
using Skimart.Application.Gateways.Payment;
using Skimart.Domain.Entities.Basket;
using Stripe;

namespace Skimart.Infrastructure.Payment.Services;

public class StripePaymentGateway : IPaymentGateway
{
    private readonly ILogger<StripePaymentGateway> _logger;
    private readonly PaymentConfiguration _paymentConfig;

    public StripePaymentGateway(ILogger<StripePaymentGateway> logger ,IOptions<PaymentConfiguration> paymentConfig)
    {
        _logger = logger;
        _paymentConfig = paymentConfig.Value;
    }
    
    public async Task CreateOrUpdatePaymentIntentAsync(CustomerBasket basket, decimal shippingPrice)
    {
        StripeConfiguration.ApiKey = _paymentConfig.SecretKey;
        var service = new PaymentIntentService();

        if (string.IsNullOrEmpty(basket.PaymentIntentId))
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = CalculateTotalPrice(basket, shippingPrice),
                Currency = "eur",
                PaymentMethodTypes = new List<string> { "card" },
            };
            
            var intent = await service.CreateAsync(options);
            basket.PaymentIntentId = intent.Id;
            basket.ClientSecret = intent.ClientSecret;
        }
        else
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = CalculateTotalPrice(basket, shippingPrice),
            };
            
            await service.UpdateAsync(basket.PaymentIntentId, options);
        }
    }

    public PaymentResult ConfirmPayment(string bodyContent, StringValues paymentEvent)
    {
        var stripeEvent = EventUtility.ConstructEvent(bodyContent, paymentEvent, _paymentConfig.WebhookSecret);

        PaymentIntent intent;
        
        switch (stripeEvent.Type)
        {
            case "payment_intent.succeeded":
                intent = (PaymentIntent)stripeEvent.Data.Object;
                _logger.LogInformation("Payment with id {paymentIntentId} succeeded.", intent.Id);
                return PaymentResult.SuccessPayment(intent.Id);
            case "payment_intent.payment_failed":
                intent = (PaymentIntent)stripeEvent.Data.Object;
                _logger.LogInformation("Payment with id {paymentIntentId} failed.", intent.Id);
                return PaymentResult.FailedPayment(intent.Id);
            default:
                _logger.LogWarning("Event type {eventType} handling not implemented.", stripeEvent.Type);
                throw new NotImplementedException();
        }
    }
    
    private static long CalculateTotalPrice(CustomerBasket basket, decimal shippingPrice)
    {
        var itemsPrice = (long)basket.Items.Sum(i => i.Quantity * (i.Price * 100));
        var shippingPriceForTotal = (long)shippingPrice * 100;

        return itemsPrice + shippingPriceForTotal;
    }
}