using AutoFixture;
using ErrorOr;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Skimart.Application.Basket.Commands.CreateOrUpdateBasket;
using Skimart.Application.Basket.Errors;
using Skimart.Application.Basket.Gateways;
using Skimart.Application.Basket.Mappers;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.UnitTests.Basket.Commands;

public class CreateOrUpdateBasketHandlerTests
{
    private readonly IBasketRepository _mockBasketRepository;
    private readonly CreateOrUpdateBasketHandler _sut;
    private readonly Fixture _fixture;

    public CreateOrUpdateBasketHandlerTests()
    {
        _fixture = new Fixture();
        var mockLogger = Substitute.For<ILogger<CreateOrUpdateBasketHandler>>();
        _mockBasketRepository = Substitute.For<IBasketRepository>();
        _sut = new CreateOrUpdateBasketHandler(mockLogger, _mockBasketRepository);
    }

    [Fact]
    public async Task Handle_WhenBasketIsUpdatedOrCreated_ReturnsBasket()
    {
        var command = _fixture.Create<CreateOrUpdateBasketCommand>();
        var basket = command.ToDomain();

        _mockBasketRepository.CreateOrUpdateBasketAsync(Arg.Any<CustomerBasket>()).Returns(basket);

        var result = await _sut.Handle(command, default);

        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(basket);
    }
    
    [Fact]
    public async Task Handle_WhenBasketFailsToUpdatedOrCreat_ReturnsError()
    {
        var command = _fixture.Create<CreateOrUpdateBasketCommand>();
        CustomerBasket? basket = null;

        _mockBasketRepository.CreateOrUpdateBasketAsync(Arg.Any<CustomerBasket>()).Returns(basket);

        var result = await _sut.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Failure);
        result.FirstError.Description.Should().Be(BasketErrors.FailedToUpdateOrCreateBasket);
    }
}