using AutoFixture;
using FluentAssertions;
using Skimart.Domain.Entities.Order;

namespace Skimart.Domain.UnitTests.Entities.Orders;

public class OrderTests
{
    private readonly Fixture _fixture;
    private readonly Order _sut;

    public OrderTests()
    {
        _fixture = new Fixture();

        var buyerEmail = _fixture.Create<string>();
        var shippingAddress = _fixture.Create<ShippingAddress>();
        var deliveryMethod = _fixture.Create<DeliveryMethod>();
        var paymentIntent = _fixture.Create<string>();
        
        _sut = new Order(buyerEmail, shippingAddress, deliveryMethod, paymentIntent);
    }

    [Fact]
    public void AddOrUpdateOrderItems_WhenNoOrderItemsYet_ShouldAddProducts()
    {
        var orderItems = _fixture.CreateMany<OrderItem>().ToList();
        
        _sut.AddOrUpdateOrderItems(orderItems);

        _sut.OrderItems.Should().BeEquivalentTo(orderItems);
    }
    
    [Fact]
    public void AddOrUpdateOrderItems_WhenOrderItemsAlreadyExist_ShouldUpdateOrderItemsAndDeleteNonExistingOnes()
    {
        var firstItem = _fixture.Create<OrderItem>();
        var secondItem = _fixture.Create<OrderItem>();
        var thirdItem = _fixture.Create<OrderItem>();

        var orderItems = new List<OrderItem>
        {
            firstItem,
            secondItem,
            thirdItem
        };
        
        _sut.AddOrUpdateOrderItems(orderItems);
        
        var firstItemUpdated = _fixture.Build<OrderItem>()
            .With(orderItem => orderItem.Id, firstItem.Id)
            .With(orderItem => orderItem.Quantity, firstItem.Quantity + 2)
            .Create();
        
        var orderItemsUpdated = new List<OrderItem>
        {
            firstItem,
            thirdItem
        };
        
        _sut.AddOrUpdateOrderItems(orderItemsUpdated);

        _sut.OrderItems.Count.Should().Be(orderItemsUpdated.Count);
        _sut.OrderItems.First().Should().BeEquivalentTo(firstItem);
        _sut.OrderItems.Last().Should().BeEquivalentTo(thirdItem);
    }
    
}