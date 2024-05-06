using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skimart.Responses;
using Skimart.Responses.ErrorResponses;

namespace Skimart.Controllers;

[AllowAnonymous]
[Route("errors/{code}")]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : BaseController
{
    public IActionResult Error(int code)
    {
        return new ObjectResult(new BaseResponse(false, code));
    }
}