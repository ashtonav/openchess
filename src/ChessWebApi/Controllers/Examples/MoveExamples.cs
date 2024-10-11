namespace ChessWebApi.Controllers.Examples;
using Chess.Core.Models;
using Swashbuckle.AspNetCore.Filters;

public class MoveExamples : IMultipleExamplesProvider<ChessMoveRequest>
{
    public IEnumerable<SwaggerExample<ChessMoveRequest>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "Example 1",
            "Example 1 - Make your first move in chess",
            new ChessMoveRequest
            {
                Move = "e2e4"
            });

        yield return SwaggerExample.Create(
            "Example 2",
            "Example 2 - Make your second move in chess",
            new ChessMoveRequest
            {
                Move = "d2d4",
                Fen = "rnbqkbnr/pp1ppppp/8/2p5/4P3/8/PPPP1PPP/RNBQKBNR w KQkq c6 0 2"
            });

    }
}
