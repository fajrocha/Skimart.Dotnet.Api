using Microsoft.AspNetCore.Identity;

namespace Skimart.Application.Abstractions.Auth;

public interface ISignInManager<TUser>
{
    Task<SignInResult> CheckPasswordSignInAsync(TUser user, string password, bool lockoutOnFailure);
}