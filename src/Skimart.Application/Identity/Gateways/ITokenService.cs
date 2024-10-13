using Skimart.Domain.Entities.Auth;

namespace Skimart.Application.Identity.Gateways;

public interface ITokenService
{
    string CreateToken(AppUser appUser);
}