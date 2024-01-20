using Microsoft.AspNetCore.Cors.Infrastructure;
using Skimart.Application.Configurations.Cors;

namespace Skimart.Extensions.Cors;

public static class CorsExtensions
{
    public static void AddCorsPolicies(this WebApplicationBuilder builder)
    {
        var corsPolicy = builder.Configuration.GetSection("CorsPolicy").Get<AppCorsPolicy>() ??
                           throw new InvalidOperationException("No CORS policies defined in settings.");

        builder.Services.AddCors(opts =>
        {
            opts.AddPolicy(corsPolicy.PolicyName, p => p.WithOrigins(corsPolicy.Origins)
                                                        .AllowAnyHeader()
                                                        .AllowAnyMethod());
        });
    }
    
    public static void AddCorsPolicies(this WebApplication app)
    {
        var corsPolicy = app.Configuration.GetSection("CorsPolicy").Get<AppCorsPolicy>() ??
                           throw new InvalidOperationException("No CORS policy defined in settings.");

        app.UseCors(corsPolicy.PolicyName);
    }
}