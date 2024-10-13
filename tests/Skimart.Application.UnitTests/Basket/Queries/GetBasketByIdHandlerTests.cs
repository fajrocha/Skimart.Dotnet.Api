using AutoFixture;
using ErrorOr;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Skimart.Application.Basket.Errors;
using Skimart.Application.Basket.Gateways;
using Skimart.Application.Basket.Queries.GetBasketById;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.UnitTests.Basket.Queries;

public class GetBasketByIdHandlerTests
{
    private readonly IBasketRepository _mockBasketRepository;
    private readonly GetBasketByIdHandler _sut;
    private readonly Fixture _fixture;

    public GetBasketByIdHandlerTests()
    {
        _fixture = new Fixture();
        var mockLogger = Substitute.For<ILogger<GetBasketByIdHandler>>();
        _mockBasketRepository = Substitute.For<IBasketRepository>();
        _sut = new GetBasketByIdHandler(mockLogger, _mockBasketRepository);
    }

    [Fact]
    public async Task Handle_WhenBasketExists_ReturnsBasket()
    {
        var query = _fixture.Create<GetBasketByIdQuery>();
        var basket = _fixture.Create<CustomerBasket>();

        _mockBasketRepository.GetBasketAsync(query.Id).Returns(basket);

        var result = await _sut.Handle(query, default);

        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(basket);
    }
    
    public async Task Handle_WhenBasketDoesNotExist_ReturnsBasket()
    {
        var query = _fixture.Create<GetBasketByIdQuery>();
        CustomerBasket? basket = null;

        _mockBasketRepository.GetBasketAsync(query.Id).Returns(basket);

        var result = await _sut.Handle(query, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Description.Should().Be(BasketErrors.BasketNotFound);
    }
}