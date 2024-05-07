using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Skimart.Extensions;

namespace Skimart.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return Problem();
        }

        return errors.All(err => err.Type == ErrorType.Validation) ? 
            ValidationProblem(errors) : 
            Problem(errors.First());
    }

    private IActionResult ValidationProblem(List<Error> errors)
    {
        return ValidationProblem(errors.ToModelStateDictionary());
    }

    private IActionResult Problem(Error error)
    {
        var responseStatusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: responseStatusCode, detail: error.Description);
    }
}