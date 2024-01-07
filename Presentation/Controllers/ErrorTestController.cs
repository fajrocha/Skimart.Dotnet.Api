using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Responses.ErrorResponses;

namespace Presentation.Controllers;

[AllowAnonymous]
public class ErrorTestController : BaseController
{
    [HttpGet("validation")]
    public IActionResult ValidationErrorTest()
    {
        var response = ErrorResponse.ValidationFailed(new List<string>
        {
            "Validation reason number 1",
            "Validation reason number 2"
        });

        return BadRequest(response);
    }
    
    [HttpGet("badRequest")]
    public IActionResult BadRequestTest()
    {
        var response = ErrorResponse.BadRequest(new List<string>
        {
            "Error number 1", 
            "Error number 2"
        });

        return BadRequest(response);
    }
    
    [HttpGet("unauthorized")]
    public IActionResult UnauthorizedRequestTest()
    {
        var response = ErrorResponse.Unauthorized(new List<string>
        {
            "Unauthorized reason 1",
            "Unauthorized reason 2"
        });

        return BadRequest(response);
    }
    
    [HttpGet("notFound")]
    public IActionResult NotFoundRequestTest()
    {
        var response = ErrorResponse.NotFound(new List<string>
        {
            "Not found reason 1", 
            "Not found reason 2"
        });

        return BadRequest(response);
    }
    
    [HttpGet("exception")]
    public IActionResult ApiExceptionTest()
    {
        throw new Exception("Exception thrown for test purposes.");
    }
}