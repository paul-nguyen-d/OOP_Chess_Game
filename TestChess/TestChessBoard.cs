using CUSTOM_PROGRAM.board;
using CUSTOM_PROGRAM.pieces;
using NUnit.Framework;
using SplashKitSDK;
using System.Collections.Generic;

namespace TestChessEngine
{
    internal class TestChessBoard
    {
        private ChessBoard _board;

        [SetUp]
        public void SetUp()
        {
            _board = new ChessBoard();
        }

        [Test]
        public void AlternatingSquares()
        {
            // the squares that have both EVEN row number and EVEN column number are WHITE
            for (int i = 0; i < _board.Height; i++)
            {
                for (int j = 0; j < _board.Width; j++)
                {
                    if ((i % 2 == 0) && (j % 2 == 0))
                        Assert.AreEqual("white", _board.BoardSquares[i, j].Color);
                }
            }

        }

        [Test]
        public void BlackSquare()
        {
            Assert.AreEqual("black", _board.BoardSquares[2, 1].Color);
        }

        [Test]
        public void MovePiece()
        {
            Square startSquare = _board.GetSquare(1, 0);
            Piece knight = _board.GetPiece(startSquare); 

            Square destSquare = _board.GetSquare(2, 2);
            Move m = new Move(startSquare, destSquare);

            _board.MovePiece(m);

            // the start square no longer contains a piece
            Assert.AreEqual(null, _board.GetPiece(startSquare));
            // the piece in the destination square
            Assert.AreEqual(knight, _board.GetPiece(destSquare));
        }
    }
}
