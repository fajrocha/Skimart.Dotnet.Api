using System.Security.Claims;

namespace Skimart.Application.UnitTests.Cases.Auth.Shared;

public static class AuthShared
{
    public static ClaimsPrincipal GetValidClaimsPrincipal()
    {
        var claims = new List<Claim>() 
        { 
            new(ClaimTypes.Email, "test@email.com"),
        };
        
        var identity = new ClaimsIdentity(claims, "TestAuthType");

        return new ClaimsPrincipal(identity);
    }
}