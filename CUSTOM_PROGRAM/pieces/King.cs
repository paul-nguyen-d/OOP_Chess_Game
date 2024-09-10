using System;
using CUSTOM_PROGRAM.pieces;
using CUSTOM_PROGRAM.board;

namespace CUSTOM_PROGRAM
{
    /// <summary>
    /// King can move in both straight lines and diagonally, but only one square at a time. 
    /// It cannot move to a square that might be under attack by another piece
    /// </summary>
    public class King : Piece
    {
        private readonly int[] _candidateX = { 1, 0, -1, 0, 1, -1, 1, -1 };
        private readonly int[] _candidateY = { 0, 1, 0, -1, 1, -1, -1, 1 };

        public King(string id, GameColor color) : base(id, color)
        {
            if (color == GameColor.WHITE)
                _imagePath = "wK.png";
            else
                _imagePath = "bK.png";
        }

        public override List<Move> PossibleMoves(IBoard board)
        {
            int candidateDestX, candidateDestY;
            List<Move> legalMoves = new List<Move>();

            for (int i = 0; i < _candidateX.Length; i++)
            {
                candidateDestX = this.X + _candidateX[i];
                candidateDestY = this.Y + _candidateY[i];

                if (board.CheckValidSquare(candidateDestX, candidateDestY))
                {
                    Square candidateDestSquare = board.GetSquare(candidateDestX, candidateDestY);
                    Square startSquare = board.GetSquare(this.X, this.Y);

                    if (!SquareIsDanger(board, candidateDestSquare)) {
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
            }

            return legalMoves;
        }

        // Check if a square is under attack by another piece
        private bool SquareIsDanger(IBoard board, Square destSquare)
        {
            List<Piece> opponentPieces;
            if (this.PieceColor == GameColor.WHITE)
                opponentPieces = board.GetPiecesFromSide(GameColor.BLACK);
            else
                opponentPieces = board.GetPiecesFromSide(GameColor.WHITE);

            List<Move> opponentPossibleMoves = new List<Move>();
            foreach (Piece p in opponentPieces)
            {
                if (p.PieceId != "WK" && p.PieceId != "BK")
                    opponentPossibleMoves.AddRange(p.PossibleMoves(board));
            }
            foreach (Move m in opponentPossibleMoves)
            {
                if (m.DestSquare == destSquare)
                    return true;
            }
            return false;
        }

    }
}
