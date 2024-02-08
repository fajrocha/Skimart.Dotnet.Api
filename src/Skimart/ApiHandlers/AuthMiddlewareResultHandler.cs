using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Skimart.ApiHandlers;

public class AuthMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

    public async Task HandleAsync(
        RequestDelegate next, 
        HttpContext context, 
        AuthorizationPolicy policy, 
        PolicyAuthorizationResult authorizeResult)
    {
        if ((authorizeResult.Challenged || authorizeResult.Forbidden) && context.GetEndpoint() is null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}
