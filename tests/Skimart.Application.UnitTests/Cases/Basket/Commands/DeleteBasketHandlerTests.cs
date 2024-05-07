using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Skimart.Application.Abstractions.Memory.Basket;
using Skimart.Application.Basket.Commands.DeleteBasket;

namespace Skimart.Application.UnitTests.Cases.Basket.Commands;

public class DeleteBasketHandlerTests
{
    private readonly Mock<ILogger<DeleteBasketHandler>> _loggerMock;
    private readonly Mock<IBasketRepository> _basketReposMock;

    public DeleteBasketHandlerTests()
    {
        _loggerMock = new Mock<ILogger<DeleteBasketHandler>>();
        _basketReposMock = new Mock<IBasketRepository>();
    }
    
    [Fact]
    public async Task Handle_WhenDeleteSucceeds_ReturnsTrue()
    {
        const string basketId = "basket";
        var command = new DeleteBasketCommand(basketId);
        const bool expectedResult = true;
        
        _basketReposMock.Setup(br => br.DeleteBasketAsync(basketId)).ReturnsAsync(expectedResult);
        var handler = new DeleteBasketHandler(_loggerMock.Object, _basketReposMock.Object);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());
        
        result.IsError.Should().BeFalse();
    }
    
    [Fact]
    public async Task Handle_WhenDeleteSucceeds_ReturnsFalse()
    {
        const string basketId = "basket";
        var command = new DeleteBasketCommand(basketId);
        const bool expectedResult = false;
        
        _basketReposMock.Setup(br => br.DeleteBasketAsync(basketId)).ReturnsAsync(expectedResult);
        var handler = new DeleteBasketHandler(_loggerMock.Object, _basketReposMock.Object);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());
        
        result.IsError.Should().BeTrue();
    }
}