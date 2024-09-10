using CUSTOM_PROGRAM.pieces;

namespace CUSTOM_PROGRAM.board
{

    /// <summary>
    /// This a square cell of a board. It knows it coordinates, colors, and can contain a piece
    /// </summary>
    public class Square : IEquatable<Square>
    {
        private int _x; // x-coordinate (from 0-7)
        private int _y; // y-coordinate (from 0-7)
        private string _color;
        private Piece _piece;


        /// <param name="x">X-coordinate of the square</param>
        /// <param name="y">Y-coordinate of the square</param>
        /// <param name="squareColor">Color of the square. If true, color is white. Else, color is black</param>
        public Square(int x, int y, bool squareColor)
        {
            _x = x;
            _y = y;

            // square-color
            if (squareColor)
                _color = "white";
            else
                _color = "black";

            _piece = null;
        }

        public int X { get => _x; }
        public int Y { get => _y; }
        public string Color { get => _color; }

        public Piece PieceContained
        {
            get => _piece;
            set => _piece = value;
        }

        public bool IsOccupied
        {
            get => _piece != null;
        }

        public bool Equals(Square other)
        {
            return this.X == other.X && this.Y == other.Y;
        }
    }
}
