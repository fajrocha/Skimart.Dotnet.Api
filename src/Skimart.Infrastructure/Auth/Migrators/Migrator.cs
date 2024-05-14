using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Skimart.Application.Identity.Gateways;
using Skimart.Domain.Entities.Auth;
using Skimart.Infrastructure.Auth.DataSeed;

namespace Skimart.Infrastructure.Auth.Migrators;

public class EfAuthMigrator : IAuthMigrator
{
    private readonly AppIdentityDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;

    public EfAuthMigrator(AppIdentityDbContext dbContext, UserManager<AppUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task MigrateAsync()
    {
        await _dbContext.Database.MigrateAsync();
    }

    public async Task SeedDataAsync()
    {
        await SeedIdentityData.SeedUsersAsync(_userManager);
    }
}