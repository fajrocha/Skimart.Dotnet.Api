using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Skimart.Identity.Extensions;

public static class IdentityExtensions
{
    public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Token:Key"] ?? "")),
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Token:Issuer"],
                    ValidateAudience = false
                };
            });
        
        return builder;
    }
    
    public static WebApplicationBuilder AddAppAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(opts => 
            opts.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build());
        
        return builder;
    }
}