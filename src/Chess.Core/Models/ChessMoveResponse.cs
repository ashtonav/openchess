namespace Chess.Core.Models;

public class ChessMoveResponse
{
    public string Move { get; set; }
    public string? Fen { get; set; }
    public Status Status { get; set; }
}
