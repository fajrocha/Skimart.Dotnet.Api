namespace Skimart.Contracts.Identity.Requests;

public record AddressUpdateRequest(
    string FirstName,
    string LastName, 
    string Street, 
    string City, 
    string Province, 
    string ZipCode);