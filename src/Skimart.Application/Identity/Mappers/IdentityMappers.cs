using Skimart.Application.Identity.Commands.Register;
using Skimart.Application.Identity.Commands.UpdateAddress;
using Skimart.Domain.Entities.Auth;

namespace Skimart.Application.Identity.Mappers;

public static class IdentityMappers
{
    public static AppUser ToUser(this RegisterCommand registerCommand)
    {
        return new AppUser
        {
            DisplayName = registerCommand.DisplayName,
            UserName = registerCommand.Email,
            Email = registerCommand.Email
        };
    }
}