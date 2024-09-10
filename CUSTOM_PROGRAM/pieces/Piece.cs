using System;
using CUSTOM_PROGRAM.board;


namespace CUSTOM_PROGRAM.pieces
{
    /// <summary>
    /// This is a Piece on the board. It has a color, coordinates, unique id, path to the image, and whether it's moved
    /// </summary>
    public abstract class Piece : IEquatable<Piece>
    {
        private GameColor _color;
        private int _x; // x-coordinate (from 0-7)
        private int _y; // y-coordinate (from 0-7)
        private string _id;
        protected string _imagePath;
        private bool _moved;


        /// <param name="id">Piece's unique id</param>
        /// <param name="color">Piece's side</param>
        protected Piece(string id, GameColor color)
        {
            // initialise at position -1
            _x = -1; 
            _y = -1;
            _color = color;
            _id = id;
            _moved = false;
        }

        /// <param name="board">The current state of the board</param>
        /// <returns>All of the possible moves it could go</returns>
        public abstract List<Move> PossibleMoves(IBoard board);
        public int X
        {
            get => _x;
            set => _x = value;
        }
        public int Y
        {
            get => _y;
            set => _y = value;
        }
        public GameColor PieceColor
        {
            get => _color;
        }

        public string PieceId
        {
            get => _id;
        }

        public string ImagePath
        {
            get => _imagePath;
        }

        public void Moved()
        {
            _moved = true;
        }

        public bool IsMoved
        {
            get => _moved;
        }

        public bool Equals(Piece? other)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"{PieceId}, x: {X}, y: {Y}, color: {PieceColor}, isMoved: {IsMoved}";
        }

    }
}
