using System.Security.Claims;
using Skimart.Application.Identity.DTOs;
using Skimart.Application.Identity.Gateways;

namespace Skimart.Identity;

public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILogger<CurrentUserProvider> _logger;

    public CurrentUserProvider(IHttpContextAccessor contextAccessor, ILogger<CurrentUserProvider> logger)
    {
        _contextAccessor = contextAccessor;
        _logger = logger;
    }
    
    public CurrentUserDto? GetCurrentUserFromClaims()
    {
        _ = _contextAccessor.HttpContext ?? throw new InvalidOperationException("Cannot find http context");

        var user = _contextAccessor.HttpContext.User;
        var email = user.FindFirstValue(ClaimTypes.Email);
        var displayName = user.FindFirstValue(ClaimTypes.GivenName);

        if (IsNull(email) || IsNull(displayName))
        {
            _logger.LogWarning("Cannot find valid claims for user. Claims found: {@user}.", user.Claims);
            return default;
        }

        return new CurrentUserDto(email, displayName);
    }

    private bool IsNull<T>(T argument)
    {
        return argument is null;
    }
}