using AutoFixture;
using ErrorOr;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Skimart.Application.Identity.DTOs;
using Skimart.Application.Identity.Errors;
using Skimart.Application.Identity.Gateways;
using Skimart.Application.Orders.Errors;
using Skimart.Application.Orders.Gateways;
using Skimart.Application.Orders.Queries.GetDeliveryMethods;
using Skimart.Application.Orders.Queries.GetOrderById;
using Skimart.Domain.Entities.Order;

namespace Skimart.Application.UnitTests.Orders.Queries;

public class GetOrderByIdHandlerTests
{
    private readonly Fixture _fixture;
    private readonly IOrderRepository _mockOrderRepository;
    private readonly ICurrentUserProvider _mockCurrentUserProvider;
    private readonly GetOrderByIdHandler _sut;

    public GetOrderByIdHandlerTests()
    {
        _fixture = new Fixture();
        _mockOrderRepository = Substitute.For<IOrderRepository>();
        _mockCurrentUserProvider = Substitute.For<ICurrentUserProvider>();

        _sut = new GetOrderByIdHandler(_mockOrderRepository, _mockCurrentUserProvider);
    }

    [Fact]
    public async Task Handle_WhenCurrentUserIsNotOnToken_ShouldReturnUserNotFoundOnTokenError()
    {
        var query = _fixture.Create<GetOrderByIdQuery>();

        _mockCurrentUserProvider.GetCurrentUserFromClaims().ReturnsNull();

        var result = await _sut.Handle(query, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Description.Should().Be(IdentityErrors.UserNotFoundOnToken);
    }
    
    [Fact]
    public async Task Handle_WhenOrderIsNotFound_ShouldReturnOrderNotFoundError()
    {
        var query = _fixture.Create<GetOrderByIdQuery>();
        var currentUser = _fixture.Create<CurrentUserDto>();

        _mockCurrentUserProvider.GetCurrentUserFromClaims().Returns(currentUser);
        _mockOrderRepository.GetOrdersByIdAndEmailAsync(query.Id, currentUser.Email).ReturnsNull();

        var result = await _sut.Handle(query, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Description.Should().Be(OrderErrors.OrderNotFound);
    }
    
    [Fact]
    public async Task Handle_WhenUserAndOrderIsFound_ShouldReturnOrder()
    {
        var query = _fixture.Create<GetOrderByIdQuery>();
        var currentUser = _fixture.Create<CurrentUserDto>();
        var order = _fixture.Build<Order>()
            .With(o => o.Id, query.Id)
            .Create();

        _mockCurrentUserProvider.GetCurrentUserFromClaims().Returns(currentUser);
        _mockOrderRepository.GetOrdersByIdAndEmailAsync(query.Id, currentUser.Email).Returns(order);

        var result = await _sut.Handle(query, default);

        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(order);
    }
}