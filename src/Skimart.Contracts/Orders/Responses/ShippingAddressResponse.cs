namespace Skimart.Contracts.Orders.Responses;

public record ShippingAddressResponse(
    string FirstName,
    string LastName,
    string Street,
    string City,
    string Province,
    string ZipCode);