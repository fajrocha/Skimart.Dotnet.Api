using AutoFixture;
using ErrorOr;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Skimart.Application.Basket.Commands.CreateOrUpdateBasket;
using Skimart.Application.Basket.Errors;
using Skimart.Application.Basket.Gateways;
using Skimart.Application.Basket.Mappers;
using Skimart.Application.Products.Gateways;
using Skimart.Application.Products.Queries.GetAllProductBrands;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.UnitTests.Basket.Commands;

public class CreateOrUpdateBasketHandlerTests
{
    private readonly IBasketRepository _basketRepositoryMock;
    private readonly CreateOrUpdateBasketHandler _sut;
    private readonly Fixture _fixture;

    public CreateOrUpdateBasketHandlerTests()
    {
        _fixture = new Fixture();
        var loggerMock = Substitute.For<ILogger<CreateOrUpdateBasketHandler>>();
        _basketRepositoryMock = Substitute.For<IBasketRepository>();
        _sut = new CreateOrUpdateBasketHandler(loggerMock, _basketRepositoryMock);
    }

    [Fact]
    public async Task Handle_WhenBasketIsUpdatedOrCreated_ReturnsBasket()
    {
        var command = _fixture.Create<CreateOrUpdateBasketCommand>();
        var basket = command.ToDomain();

        _basketRepositoryMock.CreateOrUpdateBasketAsync(Arg.Any<CustomerBasket>()).Returns(basket);

        var result = await _sut.Handle(command, default);

        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(basket);
    }
    
    [Fact]
    public async Task Handle_WhenBasketFailsToUpdatedOrCreat_ReturnsError()
    {
        var command = _fixture.Create<CreateOrUpdateBasketCommand>();
        CustomerBasket? basket = null;

        _basketRepositoryMock.CreateOrUpdateBasketAsync(Arg.Any<CustomerBasket>()).Returns(basket);

        var result = await _sut.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Failure);
        result.FirstError.Description.Should().Be(BasketErrors.FailedToUpdateOrCreateBasket);
    }
}