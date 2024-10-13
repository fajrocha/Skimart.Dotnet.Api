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
using Skimart.Application.Orders.Queries.GetOrderById;
using Skimart.Application.Orders.Queries.GetOrdersByEmail;
using Skimart.Domain.Entities.Order;

namespace Skimart.Application.UnitTests.Orders.Queries;

public class GetOrdersByEmailHandlerTests
{
    private readonly Fixture _fixture;
    private readonly IOrderRepository _mockOrderRepository;
    private readonly GetOrdersByEmailHandler _sut;

    public GetOrdersByEmailHandlerTests()
    {
        _fixture = new Fixture();
        _mockOrderRepository = Substitute.For<IOrderRepository>();

        _sut = new GetOrdersByEmailHandler(_mockOrderRepository);
    }

    [Fact]
    public async Task Handle_WhenCalled_ShouldReturnOrders()
    {
        var orders = _fixture.CreateMany<Order>().ToList();
        var email = _fixture.Create<string>();
        var query = new GetOrdersByEmailQuery(email);

        _mockOrderRepository.GetOrdersByEmailAsync(email).Returns(orders);

        var result = await _sut.Handle(query, default);

        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(orders);
    }
}