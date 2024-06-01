using AutoFixture;
using ErrorOr;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Skimart.Application.Basket.Commands.DeleteBasket;
using Skimart.Application.Basket.Errors;
using Skimart.Application.Basket.Gateways;

namespace Skimart.Application.UnitTests.Basket.Commands;

public class DeleteBasketHandlerTests
{
    private readonly IBasketRepository _mockBasketRepository;
    private readonly DeleteBasketHandler _sut;
    private readonly Fixture _fixture;

    public DeleteBasketHandlerTests()
    {
        _fixture = new Fixture();
        var mockLogger = Substitute.For<ILogger<DeleteBasketHandler>>();
        _mockBasketRepository = Substitute.For<IBasketRepository>();
        _sut = new DeleteBasketHandler(mockLogger, _mockBasketRepository);
    }

    [Fact]
    public async Task Handle_WhenBasketIsDeletedSuccessfully_ReturnsDeletedResult()
    {
        const bool deletionResult = true;
        var command = _fixture.Create<DeleteBasketCommand>();

        _mockBasketRepository.DeleteBasketAsync(command.Id).Returns(deletionResult);

        var result = await _sut.Handle(command, default);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Deleted);
    }
    
    [Fact]
    public async Task Handle_WhenBasketFailsToUpdatedOrCreat_ReturnsError()
    {
        const bool deletionResult = false;
        var command = _fixture.Create<DeleteBasketCommand>();

        _mockBasketRepository.DeleteBasketAsync(command.Id).Returns(deletionResult);
        
        var result = await _sut.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Description.Should().Be(BasketErrors.FailedToDeleteBasket);
    }
}