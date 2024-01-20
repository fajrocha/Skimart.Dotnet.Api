using AutoFixture;
using DeepEqual.Syntax;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Skimart.Application.Abstractions.Memory.Basket;
using Skimart.Application.Cases.Basket.Commands.CreateOrUpdateBasket;
using Skimart.Application.Cases.Basket.Dtos;
using Skimart.Application.Cases.Products.Queries.GetAllProducts;
using Skimart.Application.Mappers;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.UnitTests.Cases.Basket.Commands;

public class CreateOrUpdateBasketHandlerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IBasketRepository> _basketReposMock;
    private readonly Mock<ILogger<CreateOrUpdateBasketHandler>> _loggerMock;
    private readonly Fixture _fixture;

    public CreateOrUpdateBasketHandlerTests()
    {
        _loggerMock = new Mock<ILogger<CreateOrUpdateBasketHandler>>();
        _basketReposMock = new Mock<IBasketRepository>();
        _mapper = AppMapper.GetMapper();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Handle_WhenCreateOrUpdateSucceeds_ReturnsSuccessAndBasket()
    {
        var itemsDto = new List<BasketItemDto>
        {
            new BasketItemDto(1, "Example Product", 19.99m, 2, "https://example.com/image.jpg","Example Brand", 
                "Example Type"),
        };
        const string id = "basketId";
        const decimal shippingPrice = 1.12m;
        const int deliveryMethod = 1;
        var basketDto = new CustomerBasketDto(id, itemsDto, shippingPrice, deliveryMethod);
        var command = new CreateOrUpdateBasketCommand(basketDto);
        
        var items = new List<BasketItem>
        {
            new BasketItem
            {
                Id = 1,
                ProductName = "Example Product",
                Price = 19.99m,
                Quantity = 2,
                PictureUrl = "https://example.com/image.jpg",
                Brand = "Example Brand",
                Type = "Example Type"
            },
        };
        
        var basket = new CustomerBasket
        {
            Id = id,
            Items = items,
            DeliveryMethodId = 1,
            ShippingPrice = shippingPrice
        };

        _basketReposMock.Setup(br => br.CreateOrUpdateBasketAsync(It.IsAny<CustomerBasket>()))
            .ReturnsAsync(basket);

        var handler = new CreateOrUpdateBasketHandler(_loggerMock.Object, _basketReposMock.Object, _mapper);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        var actualBasket = result.Value;
        Assert.True(result.IsSuccess);
        actualBasket.ShouldDeepEqual(basket);
    }
    
    [Fact]
    public async Task Handle_WhenCreateOrUpdateFails_ReturnsFailure()
    {
        var itemsDto = new List<BasketItemDto>
        {
            new BasketItemDto(1, "Example Product", 19.99m, 2, "https://example.com/image.jpg","Example Brand", 
                "Example Type"),
        };
        const string id = "basketId";
        const decimal shippingPrice = 1.12m;
        const int deliveryMethod = 1;
        var basketDto = new CustomerBasketDto(id, itemsDto, shippingPrice, deliveryMethod);
        var command = new CreateOrUpdateBasketCommand(basketDto);

        CustomerBasket? basket = null;
        _basketReposMock.Setup(br => br.CreateOrUpdateBasketAsync(It.IsAny<CustomerBasket>()))
            .ReturnsAsync(basket);

        var handler = new CreateOrUpdateBasketHandler(_loggerMock.Object, _basketReposMock.Object, _mapper);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        Assert.True(result.IsFailed);
    }
}