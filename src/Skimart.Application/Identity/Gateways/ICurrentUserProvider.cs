using Skimart.Application.Identity.DTOs;

namespace Skimart.Application.Identity.Gateways;

public interface ICurrentUserProvider
{
    CurrentUserDto? GetCurrentUserFromClaims();
}