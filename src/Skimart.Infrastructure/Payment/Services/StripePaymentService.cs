using FluentResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Skimart.Application.Abstractions.Payment;
using Skimart.Application.Configurations.Payment;
using Skimart.Domain.Entities.Basket;
using Stripe;

namespace Skimart.Infrastructure.Payment.Services;

public class StripePaymentService : IPaymentService
{
    private readonly ILogger<StripePaymentService> _logger;
    private readonly PaymentConfiguration _paymentConfig;

    public StripePaymentService(ILogger<StripePaymentService> logger ,IOptions<PaymentConfiguration> paymentConfig)
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

    public Result ConfirmPayment(string bodyContent, StringValues paymentEvent)
    {
        throw new NotImplementedException();
    }
    
    private static long CalculateTotalPrice(CustomerBasket basket, decimal shippingPrice)
    {
        var itemsPrice = (long)basket.Items.Sum(i => i.Quantity * (i.Price * 100));
        var shippingPriceForTotal = (long)shippingPrice * 100;

        return itemsPrice + shippingPriceForTotal;
    }
}