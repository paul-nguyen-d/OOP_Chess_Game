using CUSTOM_PROGRAM.pieces;
using CUSTOM_PROGRAM.board;

namespace CUSTOM_PROGRAM
{
    /// <summary>
    /// The Bishop can move and kill another piece diagonally
    /// </summary>
    public class Bishop : Piece
    {
        private readonly int[] _candidateVectorX = { 1, -1, 1, -1 };
        private readonly int[] _candidateVectorY = { 1, -1, -1, 1 };

        public Bishop(string id, GameColor color) : base(id, color)
        {
            if (color == GameColor.WHITE)
                _imagePath = "wB.png";
            else
                _imagePath = "bB.png";
        }

        public override List<Move> PossibleMoves(IBoard board)
        {
            int candidateDestX, candidateDestY;
            List<Move> legalMoves = new List<Move>();

            for (int i=0; i < _candidateVectorX.Length; i++)
            {
                candidateDestX = this.X;
                candidateDestY = this.Y;

                while (board.CheckValidSquare(candidateDestX, candidateDestY)) {
                    candidateDestX += _candidateVectorX[i];
                    candidateDestY += _candidateVectorY[i];

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
                            break ;
                        }
                   } 

                }
            }
            return legalMoves;
        }

    }
}
