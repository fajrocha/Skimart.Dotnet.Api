using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Skimart.Application.Abstractions.Auth;
using Skimart.Application.Configurations.Auth;
using Skimart.Domain.Entities.Auth;

namespace Skimart.Infrastructure.Auth.Services;

public class JwtTokenService : ITokenService
{
    private readonly TokenConfig _tokenConfig;
    private readonly SymmetricSecurityKey _key;

    public JwtTokenService(IOptions<TokenConfig> config)
    {
        _tokenConfig = config.Value;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.Key));
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
            Issuer = _tokenConfig.Issuer
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
            
        return tokenHandler.WriteToken(token);
    }
}