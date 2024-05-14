using System.ComponentModel.DataAnnotations;

namespace Skimart.Application.Identity.DTOs;

public record AddressDto(
    [Required] string FirstName,
    [Required] string LastName, 
    [Required] string Street, 
    [Required] string City, 
    [Required] string Province, 
    [Required] string ZipCode);