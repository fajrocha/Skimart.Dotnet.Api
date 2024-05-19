namespace Skimart.Application.Cases.Orders.Commands.CreateOrder;

public record CreateOrderShippingAddressCommand(
    string FirstName,
    string LastName, 
    string Street, 
    string City, 
    string Province, 
    string ZipCode);