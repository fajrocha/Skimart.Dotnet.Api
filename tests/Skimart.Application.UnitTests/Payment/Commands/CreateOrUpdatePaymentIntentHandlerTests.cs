using AutoFixture;
using FluentAssertions;
using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.Exceptions;
using NSubstitute.ReturnsExtensions;
using Skimart.Application.Basket.Gateways;
using Skimart.Application.Orders.Gateways;
using Skimart.Application.Payment.Commands.CreateOrUpdatePaymentIntent;
using Skimart.Application.Payment.Errors;
using Skimart.Application.Payment.Gateways;
using Skimart.Application.Products.Gateways;
using Skimart.Domain.Entities.Basket;
using Skimart.Domain.Entities.Order;
using Skimart.Domain.Entities.Products;

namespace Skimart.Application.UnitTests.Payment.Commands;

public class CreateOrUpdatePaymentIntentHandlerTests
{
    private readonly Fixture _fixture;
    private readonly IDeliveryMethodRepository _mockDeliveryMethodRepository;
    private readonly IBasketRepository _mockBasketRepository;
    private readonly IProductRepository _mockProductRepository;
    private readonly IPaymentGateway _mockPaymentGateway;
    private readonly CreateOrUpdatePaymentIntentHandler _sut;

    public CreateOrUpdatePaymentIntentHandlerTests()
    {
        _fixture = new Fixture();
        _mockBasketRepository = Substitute.For<IBasketRepository>();
        _mockProductRepository = Substitute.For<IProductRepository>();
        _mockDeliveryMethodRepository = Substitute.For<IDeliveryMethodRepository>();
        _mockPaymentGateway = Substitute.For<IPaymentGateway>();

        _sut = new CreateOrUpdatePaymentIntentHandler(
            _mockBasketRepository,
            _mockProductRepository,
            _mockDeliveryMethodRepository,
            _mockPaymentGateway
        );
    }

    [Fact]
    public async Task Handle_WhenBasketDoesNotExist_ShouldReturnErrorBasketNotFound()
    {
        var command = _fixture.Create<CreateOrUpdatePaymentIntentCommand>();

        _mockBasketRepository.GetBasketAsync(command.BasketId).ReturnsNull();

        var result = await _sut.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Description.Should().Be(PaymentErrors.BasketNotFound);
    }
    
    [Fact]
    public async Task Handle_WhenBasketDoesNotHaveDeliveryMethod_ShouldReturnErrorNoDeliveryMethodOnBasket()
    {
        var command = _fixture.Create<CreateOrUpdatePaymentIntentCommand>();
        int? deliveryMethod = null;
        var basket = _fixture.Build<CustomerBasket>()
            .With(cb => cb.Id, command.BasketId)
            .With(cb => cb.DeliveryMethodId, deliveryMethod)
            .Create();
        
        _mockBasketRepository.GetBasketAsync(command.BasketId).Returns(basket);

        var result = await _sut.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Description.Should().Be(PaymentErrors.NoDeliveryMethodOnBasket);
    }
    
    [Fact]
    public async Task Handle_WhenDeliveryMethodDoesNotExists_ShouldReturnErrorDeliveryMethodNotFound()
    {
        var command = _fixture.Create<CreateOrUpdatePaymentIntentCommand>();
        var basket = _fixture.Build<CustomerBasket>()
            .With(cb => cb.Id, command.BasketId)
            .Create();
        
        _mockBasketRepository.GetBasketAsync(command.BasketId).Returns(basket);

        var result = await _sut.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Description.Should().Be(PaymentErrors.DeliveryMethodNotFound);
    }
    
    [Fact]
    public async Task Handle_WhenProductOnBasketDoesNotExistInStorage_ShouldReturnErrorDeliveryMethodNotFound()
    {
        var command = _fixture.Create<CreateOrUpdatePaymentIntentCommand>();
        var basket = _fixture.Build<CustomerBasket>()
            .With(cb => cb.Id, command.BasketId)
            .Create();
        var deliveryMethod = _fixture.Create<DeliveryMethod>();
        var firstProductId = basket.Items.First().Id;
        
        _mockBasketRepository.GetBasketAsync(command.BasketId).Returns(basket);
        _mockDeliveryMethodRepository.GetEntityByIdAsync((int)basket.DeliveryMethodId!).Returns(deliveryMethod);

        _mockProductRepository.GetEntityByIdAsync(firstProductId).ReturnsNull();

        var result = await _sut.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Description.Should().Be(PaymentErrors.ProductNotFound(firstProductId));
    }
    
    [Fact]
    public async Task Handle_WhenSuccessfullyRetrievedProductsFromStorage_ShouldCreatePaymentIntentAndUpdateBasket()
    {
        var command = _fixture.Create<CreateOrUpdatePaymentIntentCommand>();
        var basketItems = BasketItemsFactory();

        var firstProductId = basketItems[0].Id;
        var firstProduct = _fixture.Build<Product>().With(p => p.Id, firstProductId).Create();
        var secondProductId = basketItems[1].Id;
        var secondProduct = _fixture.Build<Product>().With(p => p.Id, secondProductId).Create();
        
        var basket = _fixture.Build<CustomerBasket>()
            .With(cb => cb.Id, command.BasketId)
            .With(cb => cb.Items, basketItems)
            .Create();
        var deliveryMethod = _fixture.Create<DeliveryMethod>();
        
        _mockBasketRepository.GetBasketAsync(command.BasketId).Returns(basket);
        _mockDeliveryMethodRepository.GetEntityByIdAsync((int)basket.DeliveryMethodId!).Returns(deliveryMethod);

        _mockProductRepository.GetEntityByIdAsync(firstProductId).Returns(firstProduct);
        _mockProductRepository.GetEntityByIdAsync(secondProductId).Returns(secondProduct);

        var result = await _sut.Handle(command, default);

        result.IsError.Should().BeFalse();
        await _mockPaymentGateway.Received(1).CreateOrUpdatePaymentIntentAsync(basket, deliveryMethod.Price);
        await _mockBasketRepository.Received(1).CreateOrUpdateBasketAsync(basket);
    }

    private List<BasketItem> BasketItemsFactory()
    {
        var firstBasketItem = new BasketItem(
            _fixture.Create<int>(), 
            _fixture.Create<string>(), 
            _fixture.Create<decimal>(), 
            _fixture.Create<int>(),
            _fixture.Create<string>(),
            _fixture.Create<string>(),
            _fixture.Create<string>());
        
        var secondBasketItem = new BasketItem(
            _fixture.Create<int>(), 
            _fixture.Create<string>(), 
            _fixture.Create<decimal>(), 
            _fixture.Create<int>(),
            _fixture.Create<string>(),
            _fixture.Create<string>(),
            _fixture.Create<string>());

        return new List<BasketItem>
        {
            firstBasketItem,
            secondBasketItem
        };
    }
}