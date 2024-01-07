namespace Presentation.Responses.ErrorResponses;

public class ApiException : BaseResponse
{
    public ApiException(string message, int statusCode, string? exMessage = null, string? exStackTrace = null) 
        : base(false, statusCode, message)
    {
        ExceptionMessage = exMessage;
        ExceptionStackTrace = exStackTrace;
    }

    public string? ExceptionMessage { get; }
    public string? ExceptionStackTrace { get; }
}