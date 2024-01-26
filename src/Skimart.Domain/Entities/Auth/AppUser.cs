using Microsoft.AspNetCore.Identity;

namespace Skimart.Domain.Entities.Auth;

public class AppUser : IdentityUser
{
    public string DisplayName { get; set; }

    public Address Address { get; set; }
}