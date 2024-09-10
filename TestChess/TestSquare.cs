using NUnit.Framework;
using CUSTOM_PROGRAM.board;
using SplashKitSDK;
using CUSTOM_PROGRAM;

namespace TestChessEngine
{
    internal class TestSquare
    {
        private Square _square1, _square2;
        private Pawn _pawn;

        [SetUp]
        public void Setup()
        {
            _square1 = new Square(3, 1, true);
            _square2 = new Square(3, 2, false);
        }

        [Test]
        public void SquareRow()
        {
            Assert.AreEqual(3, _square1.X);
            Assert.AreEqual(3, _square2.X);
        }

        [Test]
        public void SquareColumn()
        {
            Assert.AreEqual(1, _square1.Y);
            Assert.AreEqual(2, _square2.Y);
        }

        [Test]
        public void SquareColor()
        {
            Assert.AreEqual(Color.White, _square1.Color);
            Assert.AreEqual(Color.Black, _square2.Color);
        }

        [Test]
        public void SquareNullPiece()
        {
            Assert.IsNull(_square1.PieceContained);
            Assert.IsNull(_square1.PieceContained);
        }
    }
}
