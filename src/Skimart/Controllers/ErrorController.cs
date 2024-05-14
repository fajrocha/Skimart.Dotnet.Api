using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skimart.Middleware;

namespace Skimart.Controllers;

[AllowAnonymous]
[Route("error")]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : BaseController
{
    public IActionResult HandleError()
    {
        return Problem();
    }
    
    [Route("{code}")]
    public IActionResult Error(int code) => Problem(statusCode: code, detail: code.ToDefaultDetailMessage());
}