using CUSTOM_PROGRAM.board;
using CUSTOM_PROGRAM.pieces;

namespace CUSTOM_PROGRAM
{
    /// <summary>
    /// Knight can move and kill another piece in a L shape
    /// </summary>
    public class Knight : Piece
    {
        private readonly int[] _candidateX = { -2, -1, 1, 2, 2, 1, -1, -2 };
        private readonly int[] _candidateY = { -1, -2, -2, -1, 1, 2, 2, 1 };

        public Knight(string id, GameColor color) : base(id, color)
        {
            if (color == GameColor.WHITE)
                _imagePath = "wN.png";
            else
                _imagePath = "bN.png";
        }

        public override List<Move> PossibleMoves(IBoard board)
        {
            int candidateDestX, candidateDestY;
            List<Move> legalMoves = new List<Move>();

            for (int i=0; i < _candidateX.Length; i++)
            {
                candidateDestX = this.X + _candidateX[i];
                candidateDestY = this.Y + _candidateY[i];

                if (board.CheckValidSquare(candidateDestX, candidateDestY))
                {
                    Square candidateDestSquare = board.GetSquare(candidateDestX, candidateDestY);
                    Square startSquare = board.GetSquare(this.X, this.Y);

                    if (!candidateDestSquare.IsOccupied)
                        legalMoves.Add(new Move(startSquare, candidateDestSquare));
                    else
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

            return legalMoves;
        }

    }
}
