namespace ChessWebApi.Controllers;
using Chess.Core.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    [Route("error")]
    public ErrorModel Error()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = context.Error;

        var code = exception switch
        {
            _ => StatusCodes.Status400BadRequest
        };

        Response.StatusCode = code; // You can use HttpStatusCode enum instead

        return new ErrorModel(exception);
    }
}
