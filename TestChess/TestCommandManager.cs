using CUSTOM_PROGRAM.board;
using CUSTOM_PROGRAM.pieces;
using NUnit.Framework;

namespace TestChessEngine
{
    internal class TestCommandManager
    {
        private ChessBoard _board;
        private CommandManager _commandManager;
        private Square _startSquare, _destSquare;
        private Move _move;

        [SetUp]
        public void SetUp()
        {
            _board = new ChessBoard();
            _commandManager = new CommandManager();

            _startSquare = _board.GetSquare(1, 0);
            _destSquare = _board.GetSquare(2, 2);
            _move = new Move(_startSquare, _destSquare);

        }

        [Test]
        public void ExecuteCommand()
        { 
            Piece knight = _board.GetPiece(_startSquare);

            _commandManager.ExecuteCommand(_board, _move);

            // the start square no longer contains a piece
            Assert.AreEqual(null, _board.GetPiece(_startSquare));
            // the piece in the destination square
            Assert.AreEqual(knight, _board.GetPiece(_destSquare));
        }

        [Test]
        public void UndoMoveCommand()
        {
            Piece knight = _board.GetPiece(_startSquare);

            _commandManager.ExecuteCommand(_board, _move);
            _commandManager.UndoCommand();

            // the start square has the piece again
            Assert.AreEqual(knight, _board.GetPiece(_startSquare));
            // the destination square no longer has the piece
            Assert.AreEqual(null, _board.GetPiece(_destSquare));
        }

        [Test]
        public void RedoMoveCommand()
        {
            Piece knight = _board.GetPiece(_startSquare);

            _commandManager.ExecuteCommand(_board, _move);
            _commandManager.UndoCommand();
            _commandManager.RedoCommand();

            // the start square no longer contains a piece
            Assert.AreEqual(null, _board.GetPiece(_startSquare));
            // the piece in the destination square
            Assert.AreEqual(knight, _board.GetPiece(_destSquare));
        }

        [Test]
        public void UndoAttackCommand()
        {

        }

        [Test]
        public void RedoAttackCommand()
        {

        }
    }
}
