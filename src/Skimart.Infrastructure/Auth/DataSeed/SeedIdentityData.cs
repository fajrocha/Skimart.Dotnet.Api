using Microsoft.AspNetCore.Identity;
using Skimart.Domain.Entities.Auth;

namespace Skimart.Infrastructure.Auth.DataSeed;

public static class SeedIdentityData
{
    public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
    {
        if (!userManager.Users.Any())
        {
            var user = new AppUser
            {
                DisplayName = "fjr",
                Email = "fjr@test.com",
                UserName = "fjr09",
                Address = new Address
                {
                    FirstName = "Fabio",
                    LastName = "Rocha",
                    Street = "Rua Fonte do Judeu",
                    City = "Porto",
                    Province = "Porto",
                    ZipCode = "3800-250"
                }
            };

            await userManager.CreateAsync(user, "Pwd12345$");
        }
    }
}
