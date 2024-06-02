using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Skimart.Application.Orders.Gateways;
using Skimart.Application.Orders.Queries.GetDeliveryMethods;
using Skimart.Domain.Entities.Order;

namespace Skimart.Application.UnitTests.Orders.Queries;

public class GetDeliveryMethodsHandlerTests
{
    private readonly IDeliveryMethodRepository _mockDeliveryMethodsRepository;
    private readonly GetDeliveryMethodsHandler _sut;
    private readonly Fixture _fixture;

    public GetDeliveryMethodsHandlerTests()
    {
        _fixture = new Fixture();
        _mockDeliveryMethodsRepository = Substitute.For<IDeliveryMethodRepository>();

        _sut = new GetDeliveryMethodsHandler(_mockDeliveryMethodsRepository);
    }

    [Fact]
    public async Task Handle_WhenCalled_ShouldReturnDeliveryMethods()
    {
        var deliveryMethods = _fixture.CreateMany<DeliveryMethod>().ToList();
        var query = new GetDeliveryMethodsQuery();
        
        _mockDeliveryMethodsRepository.GetEntitiesAsync().Returns(deliveryMethods);

        var result = await _sut.Handle(query, default);

        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(deliveryMethods);
    }
}