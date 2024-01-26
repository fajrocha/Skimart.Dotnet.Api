using Skimart.Domain.Entities.Auth;

namespace Skimart.Application.Abstractions.Auth;

public interface ITokenService
{
    string CreateToken(AppUser user);
}