using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Skimart.Application.Basket.Gateways;
using Skimart.Application.Identity.Gateways;
using Skimart.Application.Orders.Gateways;
using Skimart.Application.Orders.Mappers;
using Skimart.Application.Products.Gateways;
using Skimart.Application.Shared.Extensions;
using Skimart.Application.Shared.Gateways;
using Skimart.Domain.Entities.Basket;
using Skimart.Domain.Entities.Order;
using Skimart.Domain.Entities.Products;
using Error = ErrorOr.Error;

namespace Skimart.Application.Orders.Commands.CreateOrder;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, ErrorOr<Order>>
{
    private readonly ILogger<CreateOrderHandler> _logger;
    private readonly IBasketRepository _basketRepos;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IDeliveryMethodRepository _deliveryMethodRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderHandler(
        ILogger<CreateOrderHandler> logger,
        IBasketRepository basketReposes,
        ICurrentUserProvider currentUserProvider,
        IProductRepository productRepository,
        IOrderRepository orderRepository,
        IDeliveryMethodRepository deliveryMethodRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _basketRepos = basketReposes;
        _currentUserProvider = currentUserProvider;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _deliveryMethodRepository = deliveryMethodRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Order>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var user = _currentUserProvider.GetCurrentUserFromClaims();

        var basketId = command.BasketId;
        var basket = await _basketRepos.GetBasketAsync(basketId);
        
        if (basket is null)
        {
            _logger.LogWarning("Basket with Id {basketId} not found to create order.", basketId);
            return Error.Failure(description: "The basket was not found.");
        }
        
        var deliveryMethodId = command.DeliveryMethodId;
        var deliveryMethod = await _deliveryMethodRepository.GetEntityByIdAsync(deliveryMethodId);

        if (deliveryMethod is null)
        {
            _logger.LogWarning(
                "Delivery method with Id {deliveryMethodId} not found to create order.",
                deliveryMethodId);
            return Error.Unexpected(description: "The delivery method selected was not found.");
        }

        var result = await GetOrderItemsFromBasket(basket);

        if (result.IsError) 
            return Error.Failure(description: result.FirstError.Description);

        var orderItems = result.Value;

        var paymentIntentId = basket.PaymentIntentId;
        var order = await _orderRepository.GetOrderByIntent(paymentIntentId);

        if (order is null)
        {
            // Create order:
            order = new Order(
                user.Email,
                command.ShippingAddress.ToAddress(),
                deliveryMethod,
                paymentIntentId);
            order.AddOrUpdateOrderItems(orderItems);
            
            await _orderRepository.AddAsync(order);
        }
        else
        {
            // Update order:
            order.AddOrUpdateOrderItems(orderItems);
            order.ShippingAddress = command.ShippingAddress.ToAddress();
            order.DeliveryMethod = deliveryMethod;
            
            _orderRepository.UpdateAsync(order);
        }
        
        var transactionResult = await _unitOfWork.CommitAsync();

        if (transactionResult.TransactionFailed())
        {
            _logger.LogError("The transaction to update the order for {userEmail} failed.", user.Email);
            return Error.Failure(description: "Failed to create or update the order.");
        }

        return order;
    }

    private async Task<ErrorOr<List<OrderItem>>> GetOrderItemsFromBasket(CustomerBasket basket)
    {
        var items = new List<OrderItem>();
        
        foreach (var item in basket.Items)
        {
            var productItem = await _productRepository.GetEntityByIdAsync(item.Id);

            if (productItem is null)
                return Error.Failure("Product not found.");
            
            var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
            var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
            items.Add(orderItem);
        }

        return items;
    }
}