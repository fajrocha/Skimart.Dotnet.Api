using System.Security.Claims;
using Skimart.Domain.Entities.Auth;

namespace Skimart.Application.Identity.Gateways;

public interface IAuthService
{
    Task<AppUser?> FindUserByEmailAsync(string email);
    Task<bool> CheckPasswordAsync(AppUser appUser, string password);
    Task<bool> CreateUserAsync(AppUser appUser, string password);
    Task<AppUser?> FindUserWithAddressByEmail(string email);
    Task<bool> UpdateAddressAsync(AppUser appUser);
}