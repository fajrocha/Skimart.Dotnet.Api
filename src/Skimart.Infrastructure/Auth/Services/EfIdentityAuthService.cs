﻿using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Skimart.Application.Abstractions.Auth;
using Skimart.Domain.Entities.Auth;

namespace Skimart.Infrastructure.Auth.Services;

public class EfIdentityAuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public EfIdentityAuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<AppUser?> FindUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }
    
    public async Task<bool> CheckPasswordAsync(AppUser appUser, string password)
    {
        var result = await _signInManager.CheckPasswordSignInAsync(appUser, password, false);

        return result.Succeeded;
    }

    public async Task<bool> CreateUserAsync(AppUser appUser, string password)
    {
        var result = await _userManager.CreateAsync(appUser, password);

        return result.Succeeded;
    }

    public async Task<AppUser?> FindByEmailFromClaims(ClaimsPrincipal claims)
    {
        return await _userManager.Users.SingleOrDefaultAsync(u => u.Email == GetEmailFromClaims(claims));
    }

    public async Task<AppUser?> FindAddressByEmailFromClaims(ClaimsPrincipal claims)
    {
        return await _userManager.Users.Include(x => x.Address)
            .SingleOrDefaultAsync(u => u.Email == GetEmailFromClaims(claims));
    }

    public async Task<bool> UpdateAddressAsync(AppUser appUser, Address newAddress)
    {
        appUser.Address = newAddress;
        var result = await _userManager.UpdateAsync(appUser);
        
        return result.Succeeded;
    }

    private static string? GetEmailFromClaims(ClaimsPrincipal claims)
    {
        return claims.FindFirstValue(ClaimTypes.Email);
    }
}