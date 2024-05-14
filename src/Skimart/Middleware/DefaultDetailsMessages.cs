namespace Skimart.Middleware;

public static class DefaultDetailsMessages
{
    public static string ToDefaultDetailMessage(this int code)
    {
        return code switch
        {
            400 => "Bad request occurred.",
            401 => "Unauthorized to make the request.",
            403 => "Forbidden to make the request.",
            404 => "Resource was not found.",
            500 => "Internal error occurred.",
            _ => "Internal error occurred."
        };
    }
}