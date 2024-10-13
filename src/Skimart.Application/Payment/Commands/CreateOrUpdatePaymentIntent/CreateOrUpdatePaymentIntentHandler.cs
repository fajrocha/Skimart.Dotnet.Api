using ErrorOr;
using MediatR;
using Skimart.Application.Basket.Gateways;
using Skimart.Application.Orders.Gateways;
using Skimart.Application.Payment.Errors;
using Skimart.Application.Payment.Gateways;
using Skimart.Application.Products.Gateways;
using Skimart.Application.Shared.Gateways;
using Skimart.Domain.Entities.Basket;
using Skimart.Domain.Entities.Order;
using Skimart.Domain.Entities.Products;
using Error = ErrorOr.Error;

namespace Skimart.Application.Payment.Commands.CreateOrUpdatePaymentIntent;

public class CreateOrUpdatePaymentIntentHandler : IRequestHandler<CreateOrUpdatePaymentIntentCommand, ErrorOr<CustomerBasket>>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IProductRepository _productRepository;
    private readonly IDeliveryMethodRepository _deliveryMethodRepository;
    private readonly IPaymentGateway _paymentGateway;

    public CreateOrUpdatePaymentIntentHandler(
        IBasketRepository basketRepository,
        IProductRepository productRepository,
        IDeliveryMethodRepository deliveryMethodRepository,
        IPaymentGateway paymentGateway)
    {
        _basketRepository = basketRepository;
        _productRepository = productRepository;
        _deliveryMethodRepository = deliveryMethodRepository;
        _paymentGateway = paymentGateway;
    }
    
    public async Task<ErrorOr<CustomerBasket>> Handle(CreateOrUpdatePaymentIntentCommand command, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.GetBasketAsync(command.BasketId);

        if (basket is null)
            return Error.Failure(description: PaymentErrors.BasketNotFound);
        
        if (!basket.DeliveryMethodId.HasValue)
            return Error.Failure(description: PaymentErrors.NoDeliveryMethodOnBasket);

        var deliveryMethod = await _deliveryMethodRepository.GetEntityByIdAsync((int)basket.DeliveryMethodId);

        if (deliveryMethod is null)
            return Error.Failure(description: PaymentErrors.DeliveryMethodNotFound);

        var shippingPrice = deliveryMethod.Price;
        
        var result = await VerifyItemsPrices(basket);

        if (result.IsError)
            return result.FirstError;

        await _paymentGateway.CreateOrUpdatePaymentIntentAsync(basket, shippingPrice);
        await _basketRepository.CreateOrUpdateBasketAsync(basket);

        return basket;
    }

    private async Task<ErrorOr<Success>> VerifyItemsPrices(CustomerBasket basket)
    {
        foreach (var basketItem in basket.Items)
        {
            var itemId = basketItem.Id;
            var productItem = await _productRepository.GetEntityByIdAsync(itemId);

            if (productItem is null)
                return Error.NotFound(description: PaymentErrors.ProductNotFound(itemId));

            basketItem.VerifyAgainstStoredPrice(productItem);
        }
        
        return Result.Success;
    }
}