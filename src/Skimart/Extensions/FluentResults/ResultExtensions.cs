using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Skimart.Application.Extensions.FluentResults;
using Skimart.Responses.ErrorResponses;

namespace Skimart.Extensions.FluentResults;

public static class ResultExtensions
{
    public static ActionResult<T> ToOkOrBadRequest<T>(this Result<T> result, string? message = null)
    {
        return result.IsSuccess
            ? new OkObjectResult(result.Value)
            : new BadRequestObjectResult(ErrorResponse.BadRequest(result.GetReasonsAsCollection(), message));
    }
    
    public static ActionResult<T> ToOkOrNotFound<T>(this Result<T> result, string? message = null)
    {
        return result.IsSuccess
            ? new OkObjectResult(result.Value)
            : new NotFoundObjectResult(ErrorResponse.NotFound(result.GetReasonsAsCollection(), message));
    }
}