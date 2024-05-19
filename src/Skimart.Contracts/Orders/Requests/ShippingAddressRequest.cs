namespace Skimart.Contracts.Orders.Requests;

public record ShippingAddressRequest(
    string FirstName,
    string LastName,
    string Street,
    string City,
    string Province,
    string ZipCode);