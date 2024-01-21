using System.Net;

namespace Skimart.Responses.ErrorResponses;

public class ErrorResponse : BaseResponse
{
    private ErrorResponse(
        int statusCode,
        IEnumerable<string> reasons,
        string? message = null) : base(false, statusCode, message)
    {
        Reasons = reasons;
    }
    
    public IEnumerable<string> Reasons { get; }
    
    public static ErrorResponse BadRequest(IEnumerable<string> reasons, string? message = null)
        => new ErrorResponse((int)HttpStatusCode.BadRequest, reasons, message);
    
    public static ErrorResponse ValidationFailed(IEnumerable<string> reasons)
        => new ErrorResponse((int)HttpStatusCode.BadRequest, reasons, "Input failed validation.");
    
    public static ErrorResponse Unauthorized(IEnumerable<string> reasons, string? message = null)
        => new ErrorResponse((int)HttpStatusCode.Unauthorized, reasons, message);
    
    public static ErrorResponse NotFound(IEnumerable<string> reasons, string? message = null)
        => new ErrorResponse((int)HttpStatusCode.NotFound, reasons, message);
    
    public static ErrorResponse InternalError(IEnumerable<string> reasons, string? message = null)
        => new ErrorResponse((int)HttpStatusCode.InternalServerError, reasons, message);
}