using System;
using CUSTOM_PROGRAM.pieces;
using CUSTOM_PROGRAM.board;

namespace CUSTOM_PROGRAM
{
    /// <summary>
    /// Pawn can move one square to the fron at a time. 
    /// It can jump 2 squares if it hasn't been moved.
    /// It kills another piece 1 square diagonally.
    /// </summary>
    public class Pawn : Piece
    {
        private readonly int[] _candidateX = { 0, 0, 1, -1 };
        private readonly int[] _candidateY = { 1, 2, 1, 1 };

        public Pawn(string id, GameColor color) : base(id, color)
        {
            if (color == GameColor.WHITE)
                _imagePath = "wp.png";
            else
                _imagePath = "bp.png";
        }

        public override List<Move> PossibleMoves(IBoard board)
        {
            int candidateDestX, candidateDestY;
            List<Move> legalMoves = new List<Move>();

            for (int i = 0; i < _candidateX.Length; i++)
            {
                candidateDestX = this.X + _candidateX[i];
                candidateDestY = this.Y + GetDirection() * _candidateY[i];

                if (board.CheckValidSquare(candidateDestX, candidateDestY))
                {
                    Square candidateDestSquare = board.GetSquare(candidateDestX, candidateDestY);
                    Square startSquare = board.GetSquare(this.X, this.Y);

                    if (!candidateDestSquare.IsOccupied)
                    {
                        if (i == 0)
                            legalMoves.Add(new Move(startSquare, candidateDestSquare));

                        // the pawn can jump 2 squares if it hasn't been moved
                        else if (i == 1 && !IsMoved)
                        {
                            Square squareBehindCandidate = board.GetSquare(this.X, this.Y + GetDirection());
                            if (!squareBehindCandidate.IsOccupied && !candidateDestSquare.IsOccupied)
                                legalMoves.Add(new Move(startSquare, candidateDestSquare));
                        }
                    }

                    // pawn can attack if there is a piece in 1 diagonal square from the pawn
                    else
                    {
                        if (i == 2 || i == 3)
                        {
                            // if the piece at destination square is in enemy team
                            Piece pieceAtDest = candidateDestSquare.PieceContained;
                            if (this.PieceColor != pieceAtDest.PieceColor)
                            {
                                Move attackMove = new Move(startSquare, candidateDestSquare, candidateDestSquare);
                                legalMoves.Add(attackMove);
                            }

                        }
                    }
                }
            }

            return legalMoves;
        }

        private int GetDirection()
        {
            if (this.PieceColor == GameColor.BLACK)
                return 1;
            else
                return -1;
        }
    }
}
