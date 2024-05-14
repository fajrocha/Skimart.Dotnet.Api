namespace Skimart.Contracts.Identity.Responses;

public record AddressResponse(
    string FirstName,
    string LastName, 
    string Street, 
    string City, 
    string Province, 
    string ZipCode);