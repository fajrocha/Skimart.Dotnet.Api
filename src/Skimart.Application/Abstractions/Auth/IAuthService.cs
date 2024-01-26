using System.Security.Claims;
using Skimart.Domain.Entities.Auth;

namespace Skimart.Application.Abstractions.Auth;

public interface IAuthService
{
    Task<AppUser?> FindUserByEmailAsync(string email);
    Task<bool> CheckPasswordAsync(AppUser user, string password);
    Task<bool> CreateUserAsync(AppUser user, string password);
    Task<AppUser?> FindByEmailFromClaims(ClaimsPrincipal claims);
    Task<AppUser?> FindAddressByEmailFromClaims(ClaimsPrincipal claims);
    Task<bool> UpdateAddressAsync(AppUser user, Address newAddress);
}