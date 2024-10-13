using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Skimart.Application.Identity.Configurations;
using Skimart.Application.Identity.Gateways;
using Skimart.Domain.Entities.Auth;

namespace Skimart.Infrastructure.Identity.Services;

public class JwtTokenService : ITokenService
{
    private readonly TokenConfiguration _tokenConfiguration;
    private readonly SymmetricSecurityKey _key;

    public JwtTokenService(IOptions<TokenConfiguration> config)
    {
        _tokenConfiguration = config.Value;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfiguration.Key));
    }
    
    public string CreateToken(AppUser appUser)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, appUser.Email),
            new Claim(ClaimTypes.GivenName, appUser.DisplayName),
        };

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = credentials,
            Issuer = _tokenConfiguration.Issuer
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
            
        return tokenHandler.WriteToken(token);
    }
}