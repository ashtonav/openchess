namespace ChessWebApi.Controllers;

using Chess.Core.Extensions;
using Chess.Core.Models;
using Chess.Core.Services;
using Examples;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

[ApiController]
[Route("[controller]")]
public class MoveController(ILogger<MoveController> logger, IChessService chessService) : Controller
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerRequestExample(typeof(ChessMoveRequest), typeof(MoveExamples))]
    public async Task<ChessMoveResponse?> Create(ChessMoveRequest moveRequest)
    {
        var domain = moveRequest.ToDomain();

        // TODO: Check for null
        var afterMove = chessService.MakeMove(domain);

        return afterMove.ToResponse();
    }
}
