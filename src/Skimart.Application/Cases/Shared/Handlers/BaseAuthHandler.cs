using Skimart.Application.Abstractions.Auth;

namespace Skimart.Application.Cases.Shared.Handlers;

public abstract class BaseAuthHandler
{
    protected readonly IAuthService _authService;
    protected readonly ITokenService _tokenService;

    protected BaseAuthHandler(IAuthService authService, ITokenService tokenService)
    {
        _authService = authService;
        _tokenService = tokenService;
    }
}