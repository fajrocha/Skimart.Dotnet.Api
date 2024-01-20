using Microsoft.AspNetCore.Identity;

namespace Skimart.Application.Abstractions.Auth;

public interface IUserManager<TUser>
{
    IQueryable<TUser> Users { get; }
    Task<TUser> FindByEmailAsync(string email);
    Task<IdentityResult> CreateAsync(TUser user);
    Task<IdentityResult> UpdateAsync(TUser user);
    Task<IdentityResult> CreateAsync(TUser user, string password);
}