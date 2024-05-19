namespace Skimart.Application.Orders.Commands.CreateOrder;

public record CreateOrderShippingAddressCommand(
    string FirstName,
    string LastName, 
    string Street, 
    string City, 
    string Province, 
    string ZipCode);