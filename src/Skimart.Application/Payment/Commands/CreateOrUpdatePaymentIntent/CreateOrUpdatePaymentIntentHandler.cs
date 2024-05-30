using ErrorOr;
using MediatR;
using Skimart.Application.Basket.Gateways;
using Skimart.Application.Gateways.Payment;
using Skimart.Application.Orders.Gateways;
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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentGateway _paymentGateway;

    public CreateOrUpdatePaymentIntentHandler(
        IBasketRepository basketRepository, 
        IUnitOfWork unitOfWork,
        IPaymentGateway paymentGateway)
    {
        _basketRepository = basketRepository;
        _unitOfWork = unitOfWork;
        _paymentGateway = paymentGateway;
    }
    
    public async Task<ErrorOr<CustomerBasket>> Handle(CreateOrUpdatePaymentIntentCommand command, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.GetBasketAsync(command.BasketId);

        if (basket is null)
            return Error.Failure(description: "Customer basket was not found.");
        
        if (!basket.DeliveryMethodId.HasValue)
            return Error.Failure(description: "No delivery method on basket.");

        var deliveryMethod = await _unitOfWork.Repository<IDeliveryMethodRepository, DeliveryMethod>()
            .GetEntityByIdAsync((int)basket.DeliveryMethodId);

        if (deliveryMethod is null)
        {
            return Error.Failure(description: "Delivery method was not found on storage.");
        }

        var shippingPrice = deliveryMethod.Price;
        
        await VerifyItemsPrices(basket);

        await _paymentGateway.CreateOrUpdatePaymentIntentAsync(basket, shippingPrice);
        await _basketRepository.CreateOrUpdateBasketAsync(basket);

        return basket;
    }

    private async Task VerifyItemsPrices(CustomerBasket basket)
    {
        foreach (var basketItem in basket.Items)
        {
            var itemId = basketItem.Id;
            var productItem = await _unitOfWork.Repository<IProductRepository, Product>().GetEntityByIdAsync(itemId);

            if (productItem is null)
                throw new InvalidOperationException($"Product with id {itemId} was not found.");

            basketItem.VerifyPrice(productItem);
        }
    }
}