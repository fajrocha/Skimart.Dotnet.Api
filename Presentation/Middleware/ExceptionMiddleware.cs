using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Presentation.Responses.ErrorResponses;

namespace Presentation.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            const HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            _logger.LogError(ex, "Error while doing the request: {exMessage}", ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            const string defaultMessage = "Error while performing the request.";
            var response = _env.IsDevelopment()
                ? new ApiException(defaultMessage, (int)statusCode, ex.Message, ex.StackTrace)
                : new ApiException(defaultMessage, (int)statusCode);

            var jsonOpts = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            var jsonResponse = JsonSerializer.Serialize(response, jsonOpts);

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}