using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Skimart.Application.Basket.Gateways;
using Skimart.Application.Cases.Orders.Dtos;
using Skimart.Application.Cases.Orders.Errors;
using Skimart.Application.Extensions.Transaction;
using Skimart.Application.Gateways.Persistence.Repositories;
using Skimart.Application.Gateways.Persistence.Repositories.StoreOrder;
using Skimart.Application.Products.Gateways;
using Skimart.Application.Shared.Gateways;
using Skimart.Domain.Entities.Basket;
using Skimart.Domain.Entities.Order;
using Skimart.Domain.Entities.Products;

namespace Skimart.Application.Cases.Orders.Commands.CreateOrder;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Result<OrderToReturnDto>>
{
    private readonly ILogger<CreateOrderHandler> _logger;
    private readonly IBasketRepository _basketRepos;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    private record OrderInternalDto(
        string PaymentIntentId,
        ShippingAddress ShippingAddress,
        DeliveryMethod DeliveryMethod,
        decimal SubTotal,
        IReadOnlyList<OrderItem> Items,
        string BuyerEmail);

    public CreateOrderHandler(
        ILogger<CreateOrderHandler> logger,
        IBasketRepository basketReposes, 
        IUnitOfWork unitOfWork, 
        IMapper mapper)
    {
        _logger = logger;
        _basketRepos = basketReposes;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<OrderToReturnDto>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var orderDto = command.OrderDto;
        var shippingAddress = _mapper.Map<ShippingAddress>(orderDto.ShippingAddress);

        var basketId = orderDto.BasketId;
        var basket = await _basketRepos.GetBasketAsync(basketId);
        
        if (basket is null)
        {
            _logger.LogWarning("Basket with Id {basketId} not found to create order.", basketId);
            return Result.Fail(OrderError.BasketNotFound);
        }

        var result = await CheckBasketItemsAgainstRepos(basket);

        if (result.IsFailed) 
            return Result.Fail(result.Errors);

        var items = result.Value;

        var deliveryMethodId = orderDto.DeliveryMethodId;
        var deliveryMethod = await _unitOfWork.Repository<IDeliveryMethodRepository, DeliveryMethod>()
            .GetEntityByIdAsync(deliveryMethodId);

        if (deliveryMethod is null)
        {
            _logger.LogWarning(
                "Delivery method with Id {deliveryMethodId} not found to create order.",
                deliveryMethodId);
            return Result.Fail(OrderError.DeliveryMethodNotFound(deliveryMethodId));
        }
        var subTotal = items.Sum(item => item.Price * item.Quantity);

        var orderInternalDto = new OrderInternalDto(
            basket.PaymentIntentId,
            shippingAddress,
            deliveryMethod,
            subTotal,
            items,
            command.BuyerEmail);
        var order = await AddOrUpdateOrder(orderInternalDto);
        
        var transactionResult = await _unitOfWork.CompleteAsync();

        if (transactionResult.TransactionFailed())
        {
            _logger.LogError("The transaction to update the order for {buyerEmail} failed.", command.BuyerEmail);
            Result.Fail(OrderError.OrderCreateTransactionFailed);
        }

        var orderToReturn = _mapper.Map<OrderToReturnDto>(order);

        return Result.Ok(orderToReturn);
    }

    private async Task<Result<List<OrderItem>>> CheckBasketItemsAgainstRepos(CustomerBasket basket)
    {
        var items = new List<OrderItem>();
        
        foreach (var item in basket.Items)
        {
            var productItem = await _unitOfWork.Repository<IProductRepository, Product>().GetEntityByIdAsync(item.Id);

            if (productItem is null)
                return Result.Fail(OrderError.ProductIdNotFound(item.Id));
            
            var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
            var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
            items.Add(orderItem);
        }

        return items;
    }

    private async Task<Order> AddOrUpdateOrder(OrderInternalDto orderInternalDto)
    {
        var order = await _unitOfWork.Repository<IOrderRepository, Order>()
            .GetOrderByIntent(orderInternalDto.PaymentIntentId);
        
        if (order is not null)
        {
            order.ShippingAddress = orderInternalDto.ShippingAddress;
            order.DeliveryMethod = orderInternalDto.DeliveryMethod;
            order.Subtotal = orderInternalDto.SubTotal;
            _unitOfWork.Repository<IOrderRepository, Order>().UpdateAsync(order);
        }
        else
        {
            order = new Order(
                orderInternalDto.Items, 
                orderInternalDto.BuyerEmail, 
                orderInternalDto.ShippingAddress, 
                orderInternalDto.DeliveryMethod, 
                orderInternalDto.SubTotal, 
                orderInternalDto.PaymentIntentId);
            await _unitOfWork.Repository<IOrderRepository, Order>().AddAsync(order);
        }

        return order;
    }
}