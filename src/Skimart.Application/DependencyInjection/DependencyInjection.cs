using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Skimart.Application.Validation;

namespace Skimart.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var currentClass = typeof(DependencyInjection);
        
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining(currentClass);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddValidatorsFromAssemblyContaining(currentClass);
        
        return services;
    }
}