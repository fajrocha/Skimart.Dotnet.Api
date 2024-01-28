using Domain.Entities.Product;
using FluentResults;
using MediatR;
using Skimart.Application.Abstractions.Memory.Basket;
using Skimart.Application.Abstractions.Payment;
using Skimart.Application.Abstractions.Persistence.Repositories;
using Skimart.Application.Abstractions.Persistence.Repositories.StoreOrder;
using Skimart.Application.Abstractions.Persistence.Repositories.StoreProduct;
using Skimart.Application.Cases.Basket.Errors;
using Skimart.Application.Cases.Payment.Errors;
using Skimart.Domain.Entities.Basket;
using Skimart.Domain.Entities.Order;

namespace Skimart.Application.Cases.Payment.Commands.CreateOrUpdatePaymentIntent;

public class CreateOrUpdatePaymentIntentHandler : IRequestHandler<CreateOrUpdatePaymentIntentCommand, Result<CustomerBasket>>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentService _paymentService;

    public CreateOrUpdatePaymentIntentHandler(
        IBasketRepository basketRepository, 
        IUnitOfWork unitOfWork,
        IPaymentService paymentService)
    {
        _basketRepository = basketRepository;
        _unitOfWork = unitOfWork;
        _paymentService = paymentService;
    }
    
    public async Task<Result<CustomerBasket>> Handle(CreateOrUpdatePaymentIntentCommand command, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.GetBasketAsync(command.BasketId);

        if (basket is null)
            return Result.Fail(CustomerBasketError.NotFound);
        
        var deliveryCheck = await CheckDeliveryMethod(basket);

        if (deliveryCheck.IsFailed) 
            return Result.Fail(deliveryCheck.Errors);

        var shippingPrice = deliveryCheck.Value;
        
        var priceCheck = await CheckItemsPrices(basket);

        if (priceCheck.IsFailed)
            return Result.Fail(priceCheck.Errors);

        await _paymentService.CreateOrUpdatePaymentIntentAsync(basket, shippingPrice);
        
        await _basketRepository.CreateOrUpdateBasketAsync(basket);

        return Result.Ok(basket);
    }

    private async Task<Result> CheckItemsPrices(CustomerBasket basket)
    {
        foreach (var item in basket.Items)
        {
            var itemId = item.Id;
            var productItem = await _unitOfWork.Repository<IProductRepository, Product>().GetEntityByIdAsync(itemId);

            if (productItem is null)
                return Result.Fail(PaymentError.ProductIdNotFound(itemId));

            if (item.Price != productItem.Price)
                item.Price = productItem.Price;
        }

        return Result.Ok();
    }

    private async Task<Result<decimal>> CheckDeliveryMethod(CustomerBasket basket)
    {
        if (!basket.DeliveryMethodId.HasValue)
            return Result.Fail(PaymentError.DeliveryMethodNotFound);
        
        var deliveryMethod = await _unitOfWork.Repository<IDeliveryMethodRepository, DeliveryMethod>()
            .GetEntityByIdAsync((int)basket.DeliveryMethodId);

        if (deliveryMethod is null)
            return Result.Fail(PaymentError.DeliveryMethodNotFound);

        return deliveryMethod.Price;
    }
}