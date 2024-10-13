using AutoFixture;
using ErrorOr;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Skimart.Application.Basket.Errors;
using Skimart.Application.Basket.Gateways;
using Skimart.Application.Identity.DTOs;
using Skimart.Application.Identity.Errors;
using Skimart.Application.Identity.Gateways;
using Skimart.Application.Orders.Commands.CreateOrder;
using Skimart.Application.Orders.Errors;
using Skimart.Application.Orders.Gateways;
using Skimart.Application.Payment.Errors;
using Skimart.Application.Products.Errors;
using Skimart.Application.Products.Gateways;
using Skimart.Application.Shared.Gateways;
using Skimart.Domain.Entities.Basket;
using Skimart.Domain.Entities.Order;
using Skimart.Domain.Entities.Products;

namespace Skimart.Application.UnitTests.Orders.Commands;

public class CreateOrderHandlerTests
{
    private readonly IBasketRepository _basketRepos;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IDeliveryMethodRepository _deliveryMethodRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateOrderHandler _sut;
    private readonly Fixture _fixture;

    public CreateOrderHandlerTests()
    {
        _fixture = new Fixture();
        var logger = Substitute.For<ILogger<CreateOrderHandler>>();
        _basketRepos = Substitute.For<IBasketRepository>();
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _productRepository = Substitute.For<IProductRepository>();
        _orderRepository = Substitute.For<IOrderRepository>();
        _deliveryMethodRepository = Substitute.For<IDeliveryMethodRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _sut = new CreateOrderHandler(
            logger,
            _basketRepos,
            _currentUserProvider,
            _productRepository,
            _orderRepository,
            _deliveryMethodRepository,
            _unitOfWork);
    }

    [Fact]
    public async Task Handle_WhenNoUserFound_ShouldReturnUserNotFoundError()
    {
        var command = _fixture.Create<CreateOrderCommand>();
        _currentUserProvider.GetCurrentUserFromClaims().ReturnsNull();

        var result = await _sut.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
        result.FirstError.Description.Should().Be(IdentityErrors.UserNotFoundOnToken);
    }
    
    [Fact]
    public async Task Handle_WhenBasketNotFound_ShouldReturnBasketNotFoundError()
    {
        var command = _fixture.Create<CreateOrderCommand>();
        var user = _fixture.Create<CurrentUserDto>();
        _currentUserProvider.GetCurrentUserFromClaims().Returns(user);
        _basketRepos.GetBasketAsync(command.BasketId).ReturnsNull();

        var result = await _sut.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
        result.FirstError.Description.Should().Be(BasketErrors.BasketNotFound);
    }
    
    [Fact]
    public async Task Handle_WhenDeliveryMethodNotFound_ShouldReturnDeliveryMethodNotFoundError()
    {
        var command = _fixture.Create<CreateOrderCommand>();
        var user = _fixture.Create<CurrentUserDto>();
        var basket = _fixture.Build<CustomerBasket>()
            .With(cb => cb.Id, command.BasketId)
            .Create();
        _currentUserProvider.GetCurrentUserFromClaims().Returns(user);
        _basketRepos.GetBasketAsync(command.BasketId).Returns(basket);
        _deliveryMethodRepository.GetEntityByIdAsync(command.DeliveryMethodId).ReturnsNull();

        var result = await _sut.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
        result.FirstError.Description.Should().Be(PaymentErrors.DeliveryMethodNotFound);
    }
    
    [Fact]
    public async Task Handle_WhenProductNotFound_ShouldReturnDeliveryMethodNotFoundError()
    {
        var command = _fixture.Create<CreateOrderCommand>();
        var user = _fixture.Create<CurrentUserDto>();
        var basket = _fixture.Build<CustomerBasket>()
            .With(cb => cb.Id, command.BasketId)
            .Create();
        var deliveryMethod = _fixture.Build<DeliveryMethod>()
            .With(dm => dm.Id, command.DeliveryMethodId)
            .Create();
        _currentUserProvider.GetCurrentUserFromClaims().Returns(user);
        _basketRepos.GetBasketAsync(command.BasketId).Returns(basket);
        _deliveryMethodRepository.GetEntityByIdAsync(command.DeliveryMethodId).Returns(deliveryMethod);
        _productRepository.GetEntityByIdAsync(basket.Items.First().Id).ReturnsNull();

        var result = await _sut.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Failure);
        result.FirstError.Description.Should().Be(ProductErrors.ProductNotFound);
    }
    
    [Fact]
    public async Task Handle_WhenOrderIsNewButTransactionFails_ShouldReturnFailedToCreateOrderError()
    {
        var command = _fixture.Create<CreateOrderCommand>();
        var user = _fixture.Create<CurrentUserDto>();
        var basket = _fixture.Build<CustomerBasket>()
            .With(cb => cb.Id, command.BasketId)
            .Create();
        var deliveryMethod = _fixture.Build<DeliveryMethod>()
            .With(dm => dm.Id, command.DeliveryMethodId)
            .Create();
        var products = CreateProductsFromBasket(basket.Items);
        const int transactionChanges = 0;
        
        _currentUserProvider.GetCurrentUserFromClaims().Returns(user);
        _basketRepos.GetBasketAsync(command.BasketId).Returns(basket);
        _deliveryMethodRepository.GetEntityByIdAsync(command.DeliveryMethodId).Returns(deliveryMethod);
        _orderRepository.GetOrderByIntent(basket.PaymentIntentId).ReturnsNull();
        products.ForEach(product => _productRepository.GetEntityByIdAsync(product.Id).Returns(product));
        _unitOfWork.CommitAsync().Returns(transactionChanges);

        var result = await _sut.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Failure);
        result.FirstError.Description.Should().Be(OrderErrors.FailedToCreateOrder);
    }
    
    [Fact]
    public async Task Handle_WhenOrderAlreadyExistsButTransactionFails_ShouldReturnFailedToCreateOrderError()
    {
        var command = _fixture.Create<CreateOrderCommand>();
        var user = _fixture.Create<CurrentUserDto>();
        var basket = _fixture.Build<CustomerBasket>()
            .With(cb => cb.Id, command.BasketId)
            .Create();
        var paymentIntent = basket.PaymentIntentId;
        var deliveryMethod = _fixture.Build<DeliveryMethod>()
            .With(dm => dm.Id, command.DeliveryMethodId)
            .Create();
        var products = CreateProductsFromBasket(basket.Items);
        var order = CreateOrderFromProducts(products, deliveryMethod, paymentIntent);
        
        _currentUserProvider.GetCurrentUserFromClaims().Returns(user);
        _basketRepos.GetBasketAsync(command.BasketId).Returns(basket);
        _deliveryMethodRepository.GetEntityByIdAsync(command.DeliveryMethodId).Returns(deliveryMethod);
        _orderRepository.GetOrderByIntent(paymentIntent).Returns(order);
        products.ForEach(product => _productRepository.GetEntityByIdAsync(product.Id).Returns(product));
        _unitOfWork.CommitAsync().Returns(0);

        var result = await _sut.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Failure);
        result.FirstError.Description.Should().Be(OrderErrors.FailedToCreateOrder);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    public async Task Handle_WhenOrderIsNewAndTransactionSucceeds_ShouldReturnOrder(int transactionChanges)
    {
        var command = _fixture.Create<CreateOrderCommand>();
        var user = _fixture.Create<CurrentUserDto>();
        var basket = _fixture.Build<CustomerBasket>()
            .With(cb => cb.Id, command.BasketId)
            .Create();
        var paymentIntent = basket.PaymentIntentId;
        var deliveryMethod = _fixture.Build<DeliveryMethod>()
            .With(dm => dm.Id, command.DeliveryMethodId)
            .Create();
        var products = CreateProductsFromBasket(basket.Items);
        
        _currentUserProvider.GetCurrentUserFromClaims().Returns(user);
        _basketRepos.GetBasketAsync(command.BasketId).Returns(basket);
        _deliveryMethodRepository.GetEntityByIdAsync(command.DeliveryMethodId).Returns(deliveryMethod);
        _orderRepository.GetOrderByIntent(basket.PaymentIntentId).ReturnsNull();
        products.ForEach(product => _productRepository.GetEntityByIdAsync(product.Id).Returns(product));
        _unitOfWork.CommitAsync().Returns(transactionChanges);

        var result = await _sut.Handle(command, default);

        result.IsError.Should().BeFalse();
        result.Value.DeliveryMethod.Should().BeEquivalentTo(deliveryMethod);
        result.Value.PaymentIntentId.Should().Be(paymentIntent);
        var orderIds = result.Value.OrderItems.Select(oi => oi.ItemOrdered.ProductItemId).ToList();
        var basketItemsIds = basket.Items.Select(bi => bi.Id).ToList();
        orderIds.Should().BeEquivalentTo(basketItemsIds);
    }

    private Order CreateOrderFromProducts(
        List<Product> products, 
        DeliveryMethod deliveryMethod,
        string paymentIntent)
    {
        var orderItems = new List<OrderItem>();
        var email = _fixture.Create<string>();
        var shippingAddress = _fixture.Create<ShippingAddress>();
        
        products.ForEach(product =>
        {
            var orderItem = _fixture.Build<OrderItem>()
                .With(orderItem => orderItem.Id, product.Id)
                .Create();
            orderItems.Add(orderItem);
        });

        var order = new Order(email, shippingAddress, deliveryMethod, paymentIntent);
        order.AddOrUpdateOrderItems(orderItems);
        
        return order;
    }

    private List<Product> CreateProductsFromBasket(List<BasketItem> basketItems)
    {
        var collectionProducts = new List<Product>();
        
        basketItems.ForEach(basketItem =>
        {
            var product = _fixture.Build<Product>()
                .With(product => product.Id, basketItem.Id)
                .Create();
            collectionProducts.Add(product);
        });

        return collectionProducts;
    }
}