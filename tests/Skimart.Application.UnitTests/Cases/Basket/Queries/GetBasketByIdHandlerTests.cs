using DeepEqual.Syntax;
using Microsoft.Extensions.Logging;
using Moq;
using Skimart.Application.Abstractions.Memory.Basket;
using Skimart.Application.Cases.Basket.Commands.DeleteBasket;
using Skimart.Application.Cases.Basket.Queries.GetBasketById;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.UnitTests.Cases.Basket.Queries;

public class GetBasketByIdHandlerTests
{
    private readonly Mock<ILogger<GetBasketByIdHandler>> _loggerMock;
    private readonly Mock<IBasketRepository> _basketReposMock;

    public GetBasketByIdHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetBasketByIdHandler>>();
        _basketReposMock = new Mock<IBasketRepository>();
    }

    [Fact]
    public async Task Handle_WhenBasketExists_ReturnsSuccessAndBasket()
    {
        const string basketId = "basket";
        var query = new GetBasketByIdQuery(basketId);

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
        
        var expectedBasket = new CustomerBasket
        {
            Id = basketId,
            Items = items,
            DeliveryMethodId = 1,
            ShippingPrice = 1.12m
        };
        
        _basketReposMock.Setup(br => br.GetBasketAsync(basketId)).ReturnsAsync(expectedBasket);

        var handle = new GetBasketByIdHandler(_loggerMock.Object, _basketReposMock.Object);

        var result = await handle.Handle(query, It.IsAny<CancellationToken>());

        var actualBasket = result.Value;
        
        Assert.True(result.IsSuccess);
        expectedBasket.ShouldDeepEqual(actualBasket);
    }
    
    [Fact]
    public async Task Handle_WhenBasketDoesNotExist_ReturnsFailure()
    {
        const string basketId = "basket";
        var query = new GetBasketByIdQuery(basketId);

        CustomerBasket? expectedBasket = null;
        _basketReposMock.Setup(br => br.GetBasketAsync(basketId)).ReturnsAsync(expectedBasket);

        var handle = new GetBasketByIdHandler(_loggerMock.Object, _basketReposMock.Object);

        var result = await handle.Handle(query, It.IsAny<CancellationToken>());

        Assert.True(result.IsFailed);
    }
}