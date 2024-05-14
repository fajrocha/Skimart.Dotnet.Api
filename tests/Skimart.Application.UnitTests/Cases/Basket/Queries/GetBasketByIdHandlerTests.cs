using DeepEqual.Syntax;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Skimart.Application.Basket.Gateways;
using Skimart.Application.Basket.Queries.GetBasketById;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.UnitTests.Cases.Basket.Queries;

public class GetBasketByIdHandlerTests
{
    private readonly Mock<ILogger<GetBasketByIdHandler>> _loggerMock;
    private readonly Mock<IBasketRepository> _basketReposMock;
    private readonly GetBasketByIdHandler _sut;

    public GetBasketByIdHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetBasketByIdHandler>>();
        _basketReposMock = new Mock<IBasketRepository>();
        
        _sut = new GetBasketByIdHandler(_loggerMock.Object, _basketReposMock.Object);
    }

    [Fact]
    public async Task Handle_WhenBasketExists_ReturnsSuccessAndBasket()
    {
        const string basketId = "basket";
        var query = new GetBasketByIdQuery(basketId);

        var items = new List<BasketItem>
        {
            new BasketItem(id: 1, productName: "Example Product", price: 19.99m, quantity: 2,
                pictureUrl: "https://example.com/image.jpg", brand: "Example Brand", type: "Example Type"),
        };

        var expectedBasket = new CustomerBasket(basketId , items, 1.12m, 1);
        
        _basketReposMock.Setup(br => br.GetBasketAsync(basketId)).ReturnsAsync(expectedBasket);

        var result = await _sut.Handle(query, It.IsAny<CancellationToken>());

        var actualBasket = result.Value;
        result.IsError.Should().BeFalse();
        expectedBasket.Should().BeEquivalentTo(actualBasket);
    }
    
    [Fact]
    public async Task Handle_WhenBasketDoesNotExist_ReturnsFailure()
    {
        const string basketId = "basket";
        var query = new GetBasketByIdQuery(basketId);

        CustomerBasket? expectedBasket = null;
        _basketReposMock.Setup(br => br.GetBasketAsync(basketId)).ReturnsAsync(expectedBasket);

        var result = await _sut.Handle(query, It.IsAny<CancellationToken>());

        result.IsError.Should().BeTrue();
    }
}