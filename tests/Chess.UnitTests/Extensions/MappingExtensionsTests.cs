namespace Chess.UnitTests.Extensions;
using Chess.Core.Extensions;
using Chess.Core.Models;
using NUnit.Framework;

[TestFixture]
public class MappingExtensionsTests
{
    [Test]
    public void ToDomainMapsFenCorrectly()
    {
        // Arrange
        var request = new ChessMoveRequest { Fen = "e5" };
        // Act
        var result = request.ToDomain();
        // Assert
        Assert.That(result.Fen, Is.EqualTo("e5"));
    }

    [Test]
    public void ToDomainMapsMoveCorrectly()
    {
        // Arrange
        var request = new ChessMoveRequest { Move = "Qf3" };
        // Act
        var result = request.ToDomain();
        // Assert
        Assert.That(result.Move, Is.EqualTo("Qf3"));
    }

    [Test]
    public void ToResponseMapsFenCorrectly()
    {
        // Arrange
        var domainModel = new ChessMoveDomainModel { Fen = "e5" };
        // Act
        var result = domainModel.ToResponse();
        // Assert
        Assert.That(result.Fen, Is.EqualTo("e5"));
    }

    [Test]
    public void ToResponseMapsMoveCorrectly()
    {
        // Arrange
        var domainModel = new ChessMoveDomainModel { Move = "Qf3" };
        // Act
        var result = domainModel.ToResponse();
        // Assert
        Assert.That(result.Move, Is.EqualTo("Qf3"));
    }

    [Test]
    public void ToDomainThrowsWhenRequestIsNull()
    {
        // Arrange
        ChessMoveRequest request = null;
        // Act & Assert
        Assert.Throws<NullReferenceException>(() => request.ToDomain());
    }

    [Test]
    public void ToResponseThrowsWhenDomainModelIsNull()
    {
        // Arrange
        ChessMoveDomainModel domainModel = null;
        // Act & Assert
        Assert.Throws<NullReferenceException>(() => domainModel.ToResponse());
    }

    [Test]
    public void ToDomainMapsEmptyFenAndMoveCorrectly()
    {
        // Arrange
        var request = new ChessMoveRequest { Fen = "", Move = "" };
        // Act
        var result = request.ToDomain();
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Fen, Is.EqualTo(""));
            Assert.That(result.Move, Is.EqualTo(""));
        });
    }

    [Test]
    public void ToResponseMapsEmptyFenAndMoveCorrectly()
    {
        // Arrange
        var domainModel = new ChessMoveDomainModel { Fen = "", Move = "" };
        // Act
        var result = domainModel.ToResponse();
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Fen, Is.EqualTo(""));
            Assert.That(result.Move, Is.EqualTo(""));
        });
    }

    [Test]
    public void ToDomainHandlesOnlyMoveProvidedCorrectly()
    {
        // Arrange
        var request = new ChessMoveRequest { Move = "Qf3" };
        // Act
        var result = request.ToDomain();
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Fen, Is.Null.Or.Empty);
            Assert.That(result.Move, Is.EqualTo("Qf3"));
        });
    }

    [Test]
    public void ToResponseHandlesOnlyFenProvidedCorrectly()
    {
        // Arrange
        var domainModel = new ChessMoveDomainModel { Fen = "e5" };
        // Act
        var result = domainModel.ToResponse();
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Fen, Is.EqualTo("e5"));
            Assert.That(result.Move, Is.Null.Or.Empty);
        });
    }

    [Test]
    public void ToResponseHandlesOnlyMoveProvidedCorrectly()
    {
        // Arrange
        var domainModel = new ChessMoveDomainModel { Move = "Qf3" };
        // Act
        var result = domainModel.ToResponse();
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Fen, Is.Null.Or.Empty);
            Assert.That(result.Move, Is.EqualTo("Qf3"));
        });
    }

    [Test]
    public void ToResponseMapsStatusCorrectly()
    {
        // Arrange
        var expectedResult = Status.BlackWon;
        var domainModel = new ChessMoveDomainModel { Status = Status.BlackWon  };

        // Act
        var result = domainModel.ToResponse();

        // Assert
        Assert.That(result.Status, Is.EqualTo(expectedResult));
    }

    [Test]
    public void ToResponseHandlesEmptyStatusCorrectly()
    {
        // Arrange
        ChessMoveDomainModel domainModel = new() { };

        // Act
        var result = domainModel.ToResponse();

        // Assert
        Assert.That(result.Status, Is.EqualTo(null));
    }

    [Test]
    public void ToDomainMapsFenRegardlessOfCase()
    {
        // Arrange
        var request = new ChessMoveRequest { Fen = "E5" };
        // Act
        var result = request.ToDomain();
        // Assert
        Assert.That(result.Fen, Is.EqualTo("E5"));
    }

    [Test]
    public void ToDomainMapsMoveRegardlessOfCase()
    {
        // Arrange
        var request = new ChessMoveRequest { Move = "QF3" };
        // Act
        var result = request.ToDomain();
        // Assert
        Assert.That(result.Move, Is.EqualTo("QF3"));
    }

    [Test]
    public void ToResponseMapsFenRegardlessOfCase()
    {
        // Arrange
        var domainModel = new ChessMoveDomainModel { Fen = "E5" };
        // Act
        var result = domainModel.ToResponse();
        // Assert
        Assert.That(result.Fen, Is.EqualTo("E5"));
    }

    [Test]
    public void ToResponseMapsMoveRegardlessOfCase()
    {
        // Arrange
        var domainModel = new ChessMoveDomainModel { Move = "QF3" };
        // Act
        var result = domainModel.ToResponse();
        // Assert
        Assert.That(result.Move, Is.EqualTo("QF3"));
    }

    [Test]
    public void ToDomainHandlesWhitespaceInFenAndMoveCorrectly()
    {
        // Arrange
        var request = new ChessMoveRequest { Fen = " ", Move = " " };
        // Act
        var result = request.ToDomain();
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Fen, Is.EqualTo(" "));
            Assert.That(result.Move, Is.EqualTo(" "));
        });
    }

    [Test]
    public void ToResponseHandlesWhitespaceInFenAndMoveCorrectly()
    {
        // Arrange
        var domainModel = new ChessMoveDomainModel { Fen = " ", Move = " " };
        // Act
        var result = domainModel.ToResponse();
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Fen, Is.EqualTo(" "));
            Assert.That(result.Move, Is.EqualTo(" "));
        });
    }

    [Test]
    public void ToDomainHandlesSpecialCharactersInFenAndMoveCorrectly()
    {
        // Arrange
        var request = new ChessMoveRequest { Fen = "@#%^", Move = "()*" };
        // Act
        var result = request.ToDomain();
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Fen, Is.EqualTo("@#%^"));
            Assert.That(result.Move, Is.EqualTo("()*"));
        });
    }

    [Test]
    public void ToResponseHandlesSpecialCharactersInFenAndMoveCorrectly()
    {
        // Arrange
        var domainModel = new ChessMoveDomainModel { Fen = "@#%^", Move = "()*" };
        // Act
        var result = domainModel.ToResponse();
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Fen, Is.EqualTo("@#%^"));
            Assert.That(result.Move, Is.EqualTo("()*"));
        });
    }
}
