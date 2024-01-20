using System.ComponentModel.DataAnnotations;

namespace Skimart.Domain.Entities.Auth;

public class Address
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string Province { get; set; }
    public string ZipCode { get; set; }
    [Required]
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}