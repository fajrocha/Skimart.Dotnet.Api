using System.Security.Claims;
using Skimart.Application.Identity.DTOs;
using Skimart.Application.Identity.Gateways;

namespace Skimart.Identity;

public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _contextAccessor;

    public CurrentUserProvider(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }
    
    public CurrentUserDto GetCurrentUserFromClaims()
    {
        _ = _contextAccessor.HttpContext ?? throw new InvalidOperationException("Cannot find http context");
        
        var email = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
        var displayName = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.GivenName);

        return new CurrentUserDto(email, displayName);
    }
}