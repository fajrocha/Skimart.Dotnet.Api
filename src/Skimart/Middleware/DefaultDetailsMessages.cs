namespace Skimart.Middleware;

public static class DefaultDetailsMessages
{
    public static string ToDetailMessage(this int code)
    {
        return code switch
        {
            400 => "Bad request occurred.",
            401 => "Unauthorized user.",
            403 => "Forbidden user.",
            404 => "Resource was not found.",
            500 => "Internal error occurred.",
            _ => "Internal error occurred."
        };
    }
}