using MediatR;
using Skimart.Application.Identity.Gateways;

namespace Skimart.Application.Identity.Queries.CheckExistingEmail;

public class CheckExistingEmailHandler : IRequestHandler<CheckExistingEmailQuery, bool>
{
    private readonly IAuthService _authService;

    public CheckExistingEmailHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<bool> Handle(CheckExistingEmailQuery query, CancellationToken cancellationToken)
    {
        var user = await _authService.FindUserByEmailAsync(query.Email);

        return user is not null;
    }
}