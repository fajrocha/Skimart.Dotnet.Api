using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Skimart.Application.Abstractions.Memory.Basket;
using Skimart.Application.Basket.Commands.CreateOrUpdateBasket;
using Skimart.Application.Basket.Mappers;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.UnitTests.Cases.Basket.Commands;

public class CreateOrUpdateBasketHandlerTests
{
    private readonly Mock<IBasketRepository> _basketReposMock;
    private readonly Mock<ILogger<CreateOrUpdateBasketHandler>> _loggerMock;
    private readonly Fixture _fixture;

    public CreateOrUpdateBasketHandlerTests()
    {
        _loggerMock = new Mock<ILogger<CreateOrUpdateBasketHandler>>();
        _basketReposMock = new Mock<IBasketRepository>();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Handle_WhenCreateOrUpdateSucceeds_ReturnsSuccessAndBasket()
    {
        var itemsCommand = new List<BasketItemCommand>
        {
            new BasketItemCommand(
                1, 
                "Example Product", 
                19.99m, 
                2, 
                "https://example.com/image.jpg",
                "Example Brand", 
                "Example Type"),
        };
        const string id = "basketId";
        const decimal shippingPrice = 1.12m;
        const int deliveryMethod = 1;
        var command = new CreateOrUpdateBasketCommand(
            id,
            itemsCommand,
            shippingPrice,
            deliveryMethod,
            "",
            "");
        
        var expectedBasket = command.ToDomain();

        _basketReposMock.Setup(br => br.CreateOrUpdateBasketAsync(It.IsAny<CustomerBasket>()))
            .ReturnsAsync(expectedBasket);

        var handler = new CreateOrUpdateBasketHandler(_loggerMock.Object, _basketReposMock.Object);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        var actualBasket = result.Value;
        result.IsError.Should().BeFalse();
        expectedBasket.Should().BeEquivalentTo(actualBasket);
    }
    
    [Fact]
    public async Task Handle_WhenCreateOrUpdateFails_ReturnsFailure()
    {
        var itemsCommand = new List<BasketItemCommand>
        {
            new BasketItemCommand(
                1, 
                "Example Product", 
                19.99m, 
                2, 
                "https://example.com/image.jpg",
                "Example Brand", 
                "Example Type"),
        };
        const string id = "basketId";
        const decimal shippingPrice = 1.12m;
        const int deliveryMethod = 1;
        var command = new CreateOrUpdateBasketCommand(
            id,
            itemsCommand,
            shippingPrice,
            deliveryMethod,
            "",
            "");

        CustomerBasket? basket = null;
        _basketReposMock.Setup(br => br.CreateOrUpdateBasketAsync(It.IsAny<CustomerBasket>()))
            .ReturnsAsync(basket);

        var handler = new CreateOrUpdateBasketHandler(_loggerMock.Object, _basketReposMock.Object);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        result.IsError.Should().BeTrue();
    }
}