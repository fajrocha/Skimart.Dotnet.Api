using Microsoft.AspNetCore.Mvc;
using Skimart.Responses.ErrorResponses;

namespace Skimart.Extensions.Validations;

public static class ValidationControllerExtensions
{
    public static void AddControllerCallValidations(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(opts =>
        {
            opts.InvalidModelStateResponseFactory = actionContext =>
            {
                var reasons = actionContext.ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value?.Errors!)
                    .Select(x => x.ErrorMessage).ToArray();

                return new BadRequestObjectResult(ErrorResponse.ValidationFailed(reasons));
            };
        });
    }
}