namespace Chess.UnitTests.Services;

using System.ComponentModel.DataAnnotations;
using Chess.Core.Services;
using Chess.Core.Models;
using NUnit.Framework;

[TestFixture]
public class ChessServiceTests
{
    private ChessService _chessService;

    [SetUp]
    public void Setup()
    {
        _chessService = new ChessService();
    }

    [Test]
    public void MakeMoveFailsWithInvalidMoveFormat()
    {
        // Arrange
        var domain = new ChessMoveDomainModel { Fen = "e4", Move = "Qf" };
        // Act & Assert
        Assert.Throws<ValidationException>(() => _chessService.MakeMove(domain));
    }

    [Test]
    public void MakeMoveFailsWithIncorrectMove()
    {
        // Arrange
        var domain = new ChessMoveDomainModel { Fen = "e5", Move = "e7e5" };
        // Act & Assert
        Assert.Throws<ValidationException>(() => _chessService.MakeMove(domain));
    }

    [Test]
    public void MakeMoveSucceedsWithCorrectData()
    {
        // Arrange
        var domain = new ChessMoveDomainModel
        {
            Fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq e3 0 1", Move = "e7e5"
        };
        // Act
        var result = _chessService.MakeMove(domain);
        // Assert
        Assert.That(result.Fen, Is.Not.Null.Or.Empty);
    }

    [Test]
    public void MakeMoveReturnsInProgressStatusWithValidMove()
    {
        // Arrange
        var domain = new ChessMoveDomainModel
        {
            Fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq e3 0 1", Move = "e7e5"
        };
        // Act
        var result = _chessService.MakeMove(domain);
        // Assert
        Assert.That(Status.InProgress, Is.EqualTo(result.Status));
    }

    [Test]
    public void MakeMoveReturnsStalemateStatusWithStalemate()
    {
        // Arrange
        var domain = new ChessMoveDomainModel { Fen = "6k1/6P1/8/6K1/8/8/8/8 w - - 0 1", Move = "g5g6" };
        // Act
        var result = _chessService.MakeMove(domain);
        // Assert
        Assert.That(Status.Stalemate, Is.EqualTo(result.Status));
    }

    [Test]
    public void MakeMoveReturnsWhiteWonStatusWithWhiteWon()
    {
        // Arrange
        var domain = new ChessMoveDomainModel { Fen = "6k1/pppQ1ppp/8/8/8/8/PPP2PPP/R5K1 w - - 0 1", Move = "d7e8" };
        // Act
        var result = _chessService.MakeMove(domain);
        // Assert
        Assert.That(Status.WhiteWon, Is.EqualTo(result.Status));
    }

    [Test]
    public void MakeMoveReturnsBlackWonStatusWithBlackWon()
    {
        // Arrange
        var domain = new ChessMoveDomainModel { Fen = "5rk1/pppq1ppp/8/8/8/8/P1P2PPP/5K1R w - - 0 1", Move = "d7d1" };
        // Act
        var result = _chessService.MakeMove(domain);
        // Assert
        Assert.That(Status.BlackWon, Is.EqualTo(result.Status));
    }

    [Test]
    public void MakeMoveThrowsExceptionWithInvalidColumn()
    {
        // Arrange
        var domain = new ChessMoveDomainModel { Fen = "e3", Move = "y7y5" }; // 'y' is not a valid column
        // Act & Assert
        Assert.Throws<ValidationException>(() => _chessService.MakeMove(domain));
    }

    [Test]
    public void MakeMoveThrowsExceptionWithInvalidRow()
    {
        // Arrange
        var domain = new ChessMoveDomainModel { Fen = "e3", Move = "e10e5" }; // '10' is not a valid row
        // Act & Assert
        Assert.Throws<ValidationException>(() => _chessService.MakeMove(domain));
    }

    [Test]
    public void MakeMoveShouldPerformAiMoveAfterPlayerMove()
    {
        // Arrange
        var chessMove = new ChessMoveDomainModel
        {
            Fen = "rnbqkbnr/pppp2p1/8/4p3/2B1P3/8/PPPP1PPP/RNBQK1NR w KQkq - 2 5", Move = "d1f3"
        };

        var expectedFenString = "rnbqkb1r/pppp2p1/5n2/4p3/2B1P3/5Q2/PPPP1PPP/RNB1K1NR w KQkq - 4 6";

        // Act
        var returnedChessMove = _chessService.MakeMove(chessMove);

        // Assert
        Assert.That(expectedFenString, Is.EqualTo(returnedChessMove.Fen));
    }

    [Test]
    public void MakeMoveShouldThrowValidationExceptionIfColorIsIncorrect()
    {
        // Arrange
        var chessMove = new ChessMoveDomainModel
        {
            Fen = "rnbqkbnr/pppp2p1/8/4p3/2B1P3/8/PPPP1PPP/RNBQK1NR b KQkq - 2 5", Move = "d1f3"
        };

        // Act & Assert
        Assert.Throws<ValidationException>(() => _chessService.MakeMove(chessMove));
    }

    [Test]
    public void MakeMoveThrowsExceptionWithOutOfBoundsSource()
    {
        // Arrange
        var domain = new ChessMoveDomainModel { Fen = "e4", Move = "i9e5" }; // 'i9' is out of bounds
        // Act & Assert
        Assert.Throws<IndexOutOfRangeException>(() => _chessService.MakeMove(domain));
    }

    [Test]
    public void MakeMoveThrowsExceptionWithOutOfBoundsDestination()
    {
        // Arrange
        var domain = new ChessMoveDomainModel { Fen = "e4", Move = "e5i9" }; // 'i9' is out of bounds
        // Act & Assert
        Assert.Throws<ValidationException>(() => _chessService.MakeMove(domain));
    }

    [Test]
    public void MakeMoveThrowsExceptionWithNullMove()
    {
        // Arrange
        var domain = new ChessMoveDomainModel
        {
            Fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1", Move = null
        };
        // Act & Assert
        Assert.Throws<ValidationException>(() => _chessService.MakeMove(domain));
    }

    [Test]
    public void MakeMoveThrowsExceptionWithEmptyMove()
    {
        // Arrange
        var domain = new ChessMoveDomainModel
        {
            Fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1", Move = string.Empty
        };
        // Act & Assert
        Assert.Throws<ValidationException>(() => _chessService.MakeMove(domain));
    }

    [Test]
    public void MakeMoveThrowsExceptionWithSameSourceDestination()
    {
        // Arrange
        var domain = new ChessMoveDomainModel { Fen = "e4", Move = "e7e7" }; // 'e7e7' is an invalid move
        // Act & Assert
        Assert.Throws<ValidationException>(() => _chessService.MakeMove(domain));
    }

    [Test]
    public void MakeMoveThrowsExceptionWithInvalidRowCharacter()
    {
        // Arrange
        var domain = new ChessMoveDomainModel { Fen = "e4", Move = "eae5" }; // 'a' is not a valid row number
        // Act & Assert
        Assert.Throws<FormatException>(() => _chessService.MakeMove(domain));
    }

    [Test]
    public void MakeMoveThrowsExceptionWithInvalidColumnNumber()
    {
        // Arrange
        var domain = new ChessMoveDomainModel { Fen = "e4", Move = "e17e5" }; // '1' is not a standard column character
        // Act & Assert
        Assert.Throws<ValidationException>(() => _chessService.MakeMove(domain));
    }

    [Test]
    public void MakeMoveShouldReturnCorrectLastMove()
    {
        // Arrange
        var chessMove = new ChessMoveDomainModel
        {
            Fen = "rnbqkbnr/pppp1ppp/8/4p3/2B1P3/8/PPPP1PPP/RNBQK1NR w KQkq - 2 5", Move = "d1f3"
        };

        var expectedMove = "d1f3";

        // Act
        var returnedChessMove = _chessService.MakeMove(chessMove);

        // Assert
        Assert.That(expectedMove, Is.EqualTo(returnedChessMove.Move));
    }

    [Test]
    public void MakeMoveShouldReturnEmptyFenOnNewEngineWithoutFen()
    {
        // Arrange
        var chessMove = new ChessMoveDomainModel { Fen = string.Empty, Move = "e2e4" };

        // Act
        var returnedChessMove = _chessService.MakeMove(chessMove);

        // will depend on your engine implementation; may return initial FEN
        var expectedFen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1";

        // Assert
        Assert.That(expectedFen, Is.EqualTo(returnedChessMove.Fen));
    }

    [Test]
    public void MakeMoveFailsWhenWrongPlayerMakesMove()
    {
        // Arrange
        var domain = new ChessMoveDomainModel
        {
            Fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1", Move = "e2e4"
        };

        // Act & Assert
        Assert.Throws<ValidationException>(() => _chessService.MakeMove(domain));
    }

    [Test]
    public void MakeMoveShouldThrowExceptionWithInvalidColumnValue()
    {
        // Arrange
        var chessMove = new ChessMoveDomainModel
        {
            Fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1", Move = "zz1z"
        };

        // Act & Assert
        Assert.Throws<ValidationException>(() => _chessService.MakeMove(chessMove));
    }

    [Test]
    public void MakeMoveShouldThrowExceptionWithInvalidRowValue()
    {
        // Arrange
        var chessMove = new ChessMoveDomainModel
        {
            Fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1", Move = "z1zz"
        };

        // Act & Assert
        Assert.Throws<ValidationException>(() => _chessService.MakeMove(chessMove));
    }

    [Test]
    public void MakeMoveShouldThrowExceptionWithIncompleteMove()
    {
        // Arrange
        var chessMove = new ChessMoveDomainModel
        {
            Fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1", Move = "e2"
        };

        // Act & Assert
        Assert.Throws<ValidationException>(() => _chessService.MakeMove(chessMove));
    }

    [Test]
    public void MakeMoveShouldSucceedIfFenIsNullAndWhiteSpace()
    {
        // Arrange
        var domain = new ChessMoveDomainModel { Fen = null, Move = "e7e5" };

        // Act
        var result = _chessService.MakeMove(domain);

        // Assert
        Assert.That(result.Fen, Is.Not.Null.Or.Empty);
    }

    [Test]
    public void MakeMoveShouldReturnCorrectStatusAfterCheckMate()
    {
        // Arrange
        var domain = new ChessMoveDomainModel
        {
            Fen = "rnbqk1nr/pppp1Qpp/2b5/4p3/2B1P3/5N2/PPPP1PPP/RNB1K2R w KQkq -", Move = "f3f7"
        };

        // Act
        var result = _chessService.MakeMove(domain);

        // Assert
        Assert.That(result.Status, Is.EqualTo(Status.WhiteWon));
    }

    [Test]
    public void MakeMoveShouldAcceptCorrectColumnIdentifiers()
    {
        foreach (var item in new[] { "a1a2", "b1b2", "c4c5", "d6d7", "e5e6", "f2f3", "g3g4", "h6h7" })
        {
            var chessMove = new ChessMoveDomainModel
            {
                Fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1", Move = item
            };
            try
            {
                // No exception should be thrown here.
                var result = _chessService.MakeMove(chessMove);
            }
            catch
            {
                Assert.Fail($"Move {item} should be valid, but it was not.");
            }
        }
    }

    [Test]
    public void MakeMoveShouldRejectIncorrectColumnIdentifiers()
    {
        foreach (var item in new[] { "i1i2", "j1j2", "k4k5", "l6l7", "m5m6", "n2n3", "o3o4", "p6p7" })
        {
            var chessMove = new ChessMoveDomainModel
            {
                Fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1", Move = item
            };
            Assert.Throws<ValidationException>(() => _chessService.MakeMove(chessMove),
                $"Move {item} should be invalid, but it was accepted.");
        }
    }

    [Test]
    public void MakeMoveShouldAcceptCorrectRowIdentifiers()
    {
        foreach (var item in new[] { "e1e2", "e2e3", "e3e4", "e4e5", "e5e6", "e6e7", "e7e8" })
        {
            var chessMove = new ChessMoveDomainModel
            {
                Fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1", Move = item
            };
            try
            {
                // No exception should be thrown here.
                var result = _chessService.MakeMove(chessMove);
            }
            catch
            {
                Assert.Fail($"Move {item} should be valid, but it was not.");
            }
        }
    }

    [Test]
    public void MakeMoveShouldRejectIncorrectRowIdentifiers()
    {
        foreach (var item in new[] { "e0e1", "e9e8", "e10e9" })
        {
            var chessMove = new ChessMoveDomainModel
            {
                Fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1", Move = item
            };
            Assert.Throws<ValidationException>(() => _chessService.MakeMove(chessMove),
                $"Move {item} should be invalid, but it was accepted.");
        }
    }

    [Test]
    public void MakeMoveShouldMakeCorrectAiMove()
    {
        // Arrange
        var chessMove = new ChessMoveDomainModel
        {
            Fen = "rnbqkb1r/pppp1ppp/2n1pn2/1B2p3/2P1P3/5N2/PP1P1PPP/RNBQK2R w KQkq - 3 4", Move = "d2d4"
        };

        // Act
        var result = _chessService.MakeMove(chessMove);

        // Assert: This is expected AI response for given sample move. Put here AI response for move "d2d4" in initial position in your chess engine.
        Assert.That(result.Fen.EndsWith("g8f6"), Is.True, "AI did not make the correct move. Expected g8f6.");
    }

    [Test]
    public void MakeMoveShouldRecognizeCheckMate()
    {
        // Arrange
        var chessMove = new ChessMoveDomainModel
        {
            Fen = "rnbqkb1r/pppp1ppp/2n2n2/1B1Pp3/2P1P3/5N2/PP3PPP/RNBQK2R b KQkq - 0 4", Move = "f8b4"
        };

        // Act
        var result = _chessService.MakeMove(chessMove);

        // Assert: Again, the status depends on your chess engine’s perspectives. If it's mate, the status will be white won.
        Assert.That(result.Status, Is.EqualTo(Status.WhiteWon),
            "The game should recognize a checkmate. Expected Status.WhiteWon.");
    }

    [Test]
    public void MakeMoveShouldRecognizeStaleMate()
    {
        // Arrange
        var chessMove = new ChessMoveDomainModel { Fen = "6k1/6P1/8/6K1/8/8/8/8 w - - 0 54", Move = "g5g6" };

        // Act
        var result = _chessService.MakeMove(chessMove);

        // Assert: This move leads to stalemate given the FEN.
        Assert.That(result.Status, Is.EqualTo(Status.Stalemate),
            "The game should recognize a stalemate. Expected Status.Stalemate.");
    }

    [Test]
    public void MakeMoveShouldHandleEmptyFen()
    {
        // Arrange
        var chessMove = new ChessMoveDomainModel { Fen = "", Move = "e2e4" };

        // Act
        var result = _chessService.MakeMove(chessMove);

        // Assert: Make sure empty FEN creates initial board position. Update with correct FEN when e2 to e4 move performed on initial board setup.
        var expectedFen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1";

        Assert.That(result.Fen, Is.EqualTo(expectedFen),
            $"Incorrect FEN generated: {result.Fen}. Expected: {expectedFen}");
    }

    [Test]
    public void MakeMoveShouldRejectMoveAfterEndOfGame()
    {
        // Arrange
        var chessMove = new ChessMoveDomainModel { Fen = "6k1/6P1/8/6K1/8/8/8/8 w - - 0 54", Move = "g5g6" };

        // Act
        var result = _chessService.MakeMove(chessMove);

        // Assert: The game should recognize a stalemate at this point.
        Assert.That(result.Status, Is.EqualTo(Status.Stalemate),
            "The game should recognize a stalemate. Expected Status.Stalemate.");

        // The game should not permit further moves once it's over.
        chessMove = new ChessMoveDomainModel { Fen = result.Fen, Move = "any" };
        Assert.Throws<ValidationException>(() => _chessService.MakeMove(chessMove),
            $"Move {chessMove.Move} in stalemate position should be marked as invalid by engine.");
    }

    [Test]
    public void MakeMoveShouldHandleSpacesInFen()
    {
        // Arrange
        var fenWithSpaces = "6k1/6P1/8 / 6K1/8/8 / 8/8 w - - 0 54";
        var chessMove = new ChessMoveDomainModel { Fen = fenWithSpaces, Move = "g5g6" };

        // Act
        var result = _chessService.MakeMove(chessMove);

        // Assert
        Assert.That(result.Fen.Trim(), Is.EqualTo(result.Fen),
            "FEN should not contain leading, trailing or multiple spaces");
    }

    [Test]
    public void MakeMoveShouldHandleFenWithIncorrectCase()
    {
        // Arrange
        var fenWithIncorrectCase = "6K1/6P1/8/6K1/8/8/8/8 W - - 0 54";
        var chessMove = new ChessMoveDomainModel { Fen = fenWithIncorrectCase, Move = "g5g6" };

        // Act
        var result = _chessService.MakeMove(chessMove);

        // Assert
        Assert.That(result.Fen, Is.EqualTo(fenWithIncorrectCase.ToLower()), "FEN should not be case sensitive");
    }

    [Test]
    public void MakeMoveShouldReturnCorrectColorToMove()
    {
        // Arrange
        var chessMove = new ChessMoveDomainModel
        {
            Fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1", Move = "e7e5"
        };

        // Act
        var result = _chessService.MakeMove(chessMove);

        // Assert: Asserting 'w' since after 'e7e5', it's white's turn.
        StringAssert.EndsWith("w", result.Fen, "After black's move, it should be white's turn to play.");

        // Arrange
        chessMove = new ChessMoveDomainModel { Fen = result.Fen, Move = "g1f3" };

        // Act
        result = _chessService.MakeMove(chessMove);

        // Assert: Asserting 'b' since after 'g1f3', it's black's turn.
        Assert.That(result.Fen, Does.EndWith("b"), "After white's move, it should be black's turn to play.");
    }

    [Test]
    public void MakeMoveShouldRejectIfNotCurrentPlayersTurn()
    {
        // Arrange
        var chessMove = new ChessMoveDomainModel
        {
            Fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq e3 0 1", Move = "e7e5"
        };

        // Act & Assert
        Assert.Throws<ValidationException>(() => _chessService.MakeMove(chessMove),
            "Move should be rejected when it's not current player's turn.");
    }

    [Test]
    public void MakeMoveShouldRecognizeEnPassant()
    {
        // Arrange
        var chessMove = new ChessMoveDomainModel
        {
            Fen = "rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq e6 0 2", Move = "d2d4"
        };

        // Act
        var result = _chessService.MakeMove(chessMove);

        // Assert: After 'd2d4', black can perform 'en passant'.
        Assert.That(result.Fen, Does.EndWith("e6"), "The game should recognize a possible en passant capture.");
    }

    [Test]
    public void MakeMoveShouldRecognizePromotion()
    {
        // Arrange
        var chessMove = new ChessMoveDomainModel { Fen = "8/2P5/8/8/8/8/8/8 w - - 0 1", Move = "c7c8q" };

        // Act
        var result = _chessService.MakeMove(chessMove);

        // Assert: Move 'c7c8q' promoting pawn to queen, replace 'Q' with the appropriate symbol for your chess engine.
        Assert.That(result.Fen, Does.StartWith("Q"), "The game should recognize a pawn promotion.");
    }

    [Test]
    public void MakeMoveShouldRecognizeCastling()
    {
        // Arrange
        var chessMove = new ChessMoveDomainModel { Fen = "r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1", Move = "e1g1" };

        // Act
        var result = _chessService.MakeMove(chessMove);

        // Assert: After 'e1g1', white king's castling should be recognized by engine, adjust the expected FEN as per your chess engine.
        Assert.That(result.Fen, Does.StartWith("r3k2r/8/8/8/8/8/8/R4RK1"),
            "The game should recognize castling of white king.");

        // Arrange
        chessMove = new ChessMoveDomainModel { Fen = result.Fen, Move = "e8c8" };

        // Act
        result = _chessService.MakeMove(chessMove);

        // Assert: After 'e8c8', black king's castling should be recognized by engine, again adjust the expected FEN as per your chess engine.
        Assert.That(result.Fen, Does.StartWith("2kr3r/8/8/8/8/8/8/R4RK1"),
            "The game should recognize castling of black king.");
    }

    [Test]
    public void MakeMoveShouldRejectInvalidCastle()
    {
        // Arrange
        var chessMove = new ChessMoveDomainModel { Fen = "r3k2r/8/8/8/8/8/8/R3K2r w Kq - 0 1", Move = "e1g1" };

        // Act & Assert
        Assert.Throws<ValidationException>(() => _chessService.MakeMove(chessMove),
            "Move should be rejected when king-side castling is not available.");

        // Arrange
        chessMove = new ChessMoveDomainModel { Fen = "r3k2r/8/8/8/8/8/8/R3K2R b Qkq - 0 1", Move = "e8c8" };

        // Act & Assert
        Assert.Throws<ValidationException>(() => _chessService.MakeMove(chessMove),
            "Move should be rejected when queen-side castling is not available.");
    }

    [Test]
    public void MakeMoveShouldRecognizeCheck()
    {
        // Arrange
        var chessMove = new ChessMoveDomainModel
        {
            Fen = "rnbqkb1r/pppppppp/8/2n5/4P3/2N5/PPPP1PPP/R1BQKBNR b KQkq - 2 3", Move = "e7e6"
        };

        // Act
        var result = _chessService.MakeMove(chessMove);

        // Assert: Move 'e7e6' puts black in check, replace '+' with the appropriate symbol for your chess engine.
        Assert.That(result.Fen, Does.EndWith("+"), "The game should recognize a check on black.");
    }

    [Test]
    public void MakeMoveShouldCorrectlyCountFullMoves()
    {
        // Arrange
        var chessMove = new ChessMoveDomainModel
        {
            Fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1", Move = "e7e5"
        };

        // Act
        var result = _chessService.MakeMove(chessMove);

        // Assert: This should be the second move of the game.
        Assert.That(result.Fen, Does.EndWith("2"), "The game should correctly count the number of full moves.");

        // Arrange
        chessMove = new ChessMoveDomainModel { Fen = result.Fen, Move = "g1f3" };

        // Act
        result = _chessService.MakeMove(chessMove);

        // Assert: This should be the third move of the game.
        Assert.That(result.Fen, Does.EndWith("3"), "The game should correctly count the number of full moves.");
    }

    [Test]
    public void MakeMoveShouldCorrectlyCountHalfMovesForFiftyMoveRule()
    {
        // Arrange
        var chessMove = new ChessMoveDomainModel { Fen = "8/8/8/8/8/8/8/2KQk3 w - - 0 1", Move = "d1d2" };

        // Act
        var result = _chessService.MakeMove(chessMove);

        // Assert: This should be the first half-move of the game as per the Fifty-move rule.
        Assert.That(result.Fen, Does.Contain("1"),
            "The game should correctly count the number of half moves for the Fifty-move rule.");

        // Arrange
        chessMove = new ChessMoveDomainModel { Fen = result.Fen, Move = "e1d2" };

        // Act
        result = _chessService.MakeMove(chessMove);

        // Assert: This should be the second half-move of the game as per the Fifty-move rule.
        Assert.That(result.Fen, Does.Contain("2"),
            "The game should correctly count the number of half moves for the Fifty-move rule.");
    }
}
