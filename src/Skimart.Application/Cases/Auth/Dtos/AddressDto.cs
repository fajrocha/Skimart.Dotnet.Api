using System.ComponentModel.DataAnnotations;

namespace Skimart.Application.Cases.Auth.Dtos;

public record AddressDto(
    [Required] string FirstName,
    [Required] string LastName, 
    [Required] string Street, 
    [Required] string City, 
    [Required] string Province, 
    [Required] string ZipCode);