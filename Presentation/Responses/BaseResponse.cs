namespace Presentation.Responses;

public class BaseResponse
{
    public BaseResponse(bool success, int statusCode, string? message = null)
    {
        Success = success;
        Message = message ?? GetDefaultResponseForStatusCode(statusCode);
    }

    public bool Success { get; }
    public string? Message { get; }

    private static string GetDefaultResponseForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            400 => "Bad request made.",
            401 => "Unauthorized user.",
            404 => "Resource not found.",
            500 => "Internal error.",
            _ => string.Empty
        };
    }
}