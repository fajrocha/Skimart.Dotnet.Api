using Microsoft.OpenApi.Models;

namespace Skimart.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer()
            .AddSwaggerGen(c =>
            {
                const string schemeName = "Bearer";
                var securityScheme = new OpenApiSecurityScheme
                {
                    Description = "JWT Auth Bearer Scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = schemeName,
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = schemeName
                    }
                };
                
                c.AddSecurityDefinition(schemeName, securityScheme);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        securityScheme, new[] { schemeName }
                    }
                };
                
                c.AddSecurityRequirement(securityRequirement);
            });

        return services;
    }
}