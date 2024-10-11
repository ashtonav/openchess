namespace Chess.Core.Models;

public class ChessMoveDomainModel
{
    public string? Fen { get; set; }
    public string Move { get; set; }
    public Status Status { get; set; }
}
