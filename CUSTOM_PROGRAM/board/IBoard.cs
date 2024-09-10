using CUSTOM_PROGRAM.pieces;

namespace CUSTOM_PROGRAM.board
{
    /// <summary>
    /// Interface of a Board. It can be used to create many kinds of board such as chessboard, checkerboard, caro board, etc.
    /// </summary>
    public interface IBoard : ICloneable
    {
        /// <summary>
        /// Generate the board
        /// </summary>
        public void InitializeBoard();

        /// <summary>
        /// Generate and place all the pieces on the board
        /// </summary>
        public void InitializePieces();

        /// <summary>
        /// Place a piece on the board. Will overwrite the existing piece
        /// </summary>
        /// <param name="piece">The chess piece</param>
        /// <param name="square">The location to be placed</param>
        /// <returns>True if the piece is succesfully placed (i.e., the square is valid, the piece is not null, etc)</returns>
        public bool SetPiece(Piece piece, Square square);

        /// <summary>
        /// Get a piece at a given location on the board
        /// </summary>
        /// <param name="square">The location</param>
        /// <returns>The piece at the chosen square</returns>
        public Piece GetPiece(Square square);

        /// <summary>
        /// Remove a piece when it is killed by another one
        /// </summary>
        /// <param name="square">The location of the to be killed piece</param>
        /// <returns>The piece that was killed</returns>
        public Piece KillPiece(Square square);

        /// <summary>
        /// Simple move without validation. In other words, the move can be illegal
        /// </summary>
        /// <param name="move"></param>
        public void MoveWithoutCheck(Move move);

        /// <summary>
        /// Move a piece on the board
        /// </summary>
        /// <param name="move">Move from a square to a square</param>
        /// <returns>True if moved successfully, false otherwise</returns>
        public bool MovePiece(Move move);

        /// <summary>
        /// Get all the pieces of a color
        /// </summary>
        /// <param name="color">The color or side</param>
        /// <returns>List of the pieces of the same color</returns>
        public List<Piece> GetPiecesFromSide(GameColor color);

        /// <summary>
        /// Get all possible moves of a color
        /// </summary>
        /// <param name="color">The color or side</param>
        /// <returns>List of all the possible moves</returns>
        public List<Move> GetPossibleMovesFromSide(GameColor color);

        /// <summary>
        /// Get all attack moved of a color
        /// </summary>
        /// <param name="color">The color or side</param>
        /// <returns>List of all the attack moves</returns>
        public List<Move> GetAttackMovesFromSide(GameColor color);

        /// <summary>
        /// Check whether a square with x, y coordinates exists on the board
        /// </summary>
        /// <param name="x">X-coordinate (integer)</param>
        /// <param name="y">Y-coordinate (integer)</param>
        /// <returns>True if on the board, false otherwise</returns>
        public bool CheckValidSquare(int x, int y);

        /// <summary>
        /// Get a square at x, y coordinates
        /// </summary>
        /// <param name="x">X-coordinate (integer)</param>
        /// <param name="y">Y-coordiante (integer)</param>
        /// <returns>The square at the given coordinates</returns>
        public Square GetSquare(int x, int y);

        /// <summary>
        /// Get all of the remaining pieces on the board
        /// </summary>
        /// <returns>List of the remaining pieces</returns>
        public List<Piece> GetRemainingPieces();

        /// <summary>
        /// Get the width of th board
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Get the height of the board
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Get all of the squares of the board
        /// </summary>
        public Square[,] BoardSquares { get; }

        public object Clone();
    }
}
