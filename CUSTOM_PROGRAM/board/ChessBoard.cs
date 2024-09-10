using System.Collections.Generic;
using CUSTOM_PROGRAM.pieces;

namespace CUSTOM_PROGRAM.board
{
    public class ChessBoard : IBoard
    {
        private Square[,] _squares;
        private Dictionary<GameColor, King> _allKings;

        private const int ROWS = 8;
        private const int COLUMNS = 8;


        public ChessBoard()
        {
            _squares = new Square[ROWS, COLUMNS];
            _allKings = new Dictionary<GameColor, King>();

            InitializeBoard();
            InitializePieces();
        }

        public void InitializeBoard()
        {
            bool squareColor = true;
            for (int i = 0; i < COLUMNS; i++)
            {
                for (int j = 0; j < ROWS; j++)
                {
                    // alternating black/white squares
                    _squares[i, j] = new Square(i, j, squareColor);
                    squareColor = !squareColor;
                }
                squareColor = !squareColor;
            }
        }

        public void InitializePieces()
        {
            //WHITE PIECES
            SetPiece(PieceFactory.GetRook("WR1", GameColor.WHITE), _squares[0, 7]);
            SetPiece(PieceFactory.GetRook("WR2", GameColor.WHITE), _squares[7, 7]);
            SetPiece(PieceFactory.GetKnight("WN1", GameColor.WHITE), _squares[1, 7]);
            SetPiece(PieceFactory.GetKnight("WN2", GameColor.WHITE), _squares[6, 7]);
            SetPiece(PieceFactory.GetBishop("WB1", GameColor.WHITE), _squares[2, 7]);
            SetPiece(PieceFactory.GetBishop("WB2", GameColor.WHITE), _squares[5, 7]);
            SetPiece(PieceFactory.GetQueen("WQ", GameColor.WHITE), _squares[3, 7]);

            King whiteKing = PieceFactory.GetKing("WK", GameColor.WHITE) as King;
            SetKing(whiteKing, _squares[4, 7]);

            // white PAWN
            Piece[] wp = new Piece[8];
            for (int i = 0; i < 8; i++)
            {
                Piece whitePawn = PieceFactory.GetPawn("WP" + (i + 1), GameColor.WHITE);
                SetPiece(whitePawn, _squares[i, 6]);
            }


            // BLACK PIECES
            SetPiece(PieceFactory.GetRook("BR1", GameColor.BLACK), _squares[0, 0]);
            SetPiece(PieceFactory.GetRook("BR2", GameColor.BLACK), _squares[7, 0]);
            SetPiece(PieceFactory.GetKnight("BN1", GameColor.BLACK), _squares[1, 0]);
            SetPiece(PieceFactory.GetKnight("BN2", GameColor.BLACK), _squares[6, 0]);
            SetPiece(PieceFactory.GetBishop("BB1", GameColor.BLACK), _squares[2, 0]);
            SetPiece(PieceFactory.GetBishop("BB2", GameColor.BLACK), _squares[5, 0]);
            SetPiece(PieceFactory.GetQueen("BQ", GameColor.BLACK), _squares[3, 0]);

            King blackKing = PieceFactory.GetKing("BK", GameColor.BLACK) as King;
            SetKing(blackKing, _squares[4, 0]);

            // black PAWN
            Piece[] bp = new Piece[8];
            for (int i = 0; i < 8; i++)
            {
                Piece blackPawn = PieceFactory.GetPawn("BP" + (i + 1), GameColor.BLACK);
                SetPiece(blackPawn, _squares[i, 1]);
            }

        }

        public bool CheckValidSquare(int x, int y)
        {
            return x >= 0 && x < COLUMNS
                     && y >= 0 && y < ROWS;
        }
        
        // Add a piece to the board, will overwrite the existing piece
        public bool SetPiece(Piece piece, Square square)
        {
            try
            {
                if (CheckValidSquare(square.X, square.Y))
                {
                    _squares[square.X, square.Y].PieceContained = piece;
                    piece.X = square.X;
                    piece.Y = square.Y;
                    return true;
                }
                else
                    return false;
            }

            catch (NullReferenceException ex)
            {

                Console.WriteLine(ex);
                return false;
            }
        }

        // Get piece of a square
        public Piece GetPiece(Square square)
        {
            if (CheckValidSquare(square.X, square.Y))
            {
                Piece piece = _squares[square.X, square.Y].PieceContained;
                return piece;
            }

            return null;
        }


        /// <summary>
        /// Set the king of a side. If a king of this side already exists, it will be removed
        /// </summary>
        /// <param name="king">The king</param>
        /// <param name="square">The square to place the king</param>
        /// <returns>True if success, false otherwise</returns>
        public bool SetKing(King king, Square square)
        {
            if (CheckValidSquare(square.X, square.Y))
            {
                GameColor color = king.PieceColor;
                if (_allKings.ContainsKey(color))
                    RemoveKing(color);

                _allKings.Add(color, king);
                return SetPiece(king, square);
            }
            else
                return false;
        }
        /// <summary>
        /// Get the king of a side
        /// </summary>
        /// <param name="color">The side</param>
        /// <returns>The king reference. Null if there's no king for the side.</returns>
        public King GetKing(GameColor color)
        {
            if (_allKings.ContainsKey(color))
                return _allKings[color];

            return null;
        }

        /// <summary>
        /// Remove the king of a side from the board
        /// </summary>
        /// <param name="color">The side</param>
        /// <returns>The removed king</returns>
        public King RemoveKing(GameColor color)
        {
            King king = GetKing(color);
            if (king != null)
            {
                Square kingSquare = GetSquare(king.X, king.Y);
                //KillPiece(kingSquare);
                _allKings.Remove(color);
            }
            return king;
        }


        public void MoveWithoutCheck(Move move)
        {
            Square startSquare = move.StartSquare;
            Square destSquare = move.DestSquare;
            Piece movedPiece = GetPiece(startSquare);

            // let the vitim piece know it has been killed
            if (move.IsAttack)
                KillPiece(destSquare);

            SetPiece(movedPiece, destSquare);
            movedPiece.Moved();

            _squares[startSquare.X, startSquare.Y].PieceContained = null;
        }

        public bool MovePiece(Move move)
        {
            Square startSquare = move.StartSquare;
            Piece piece = GetPiece(startSquare);
            List<Move> possibleMoves = GetPiece(startSquare).PossibleMoves(this);
            if (possibleMoves.Contains(move))
            {
                MoveWithoutCheck(move);

                if (piece.PieceId[1] == 'P')
                {
                    if (((piece.PieceColor == GameColor.WHITE) && (piece.Y == 0)) ||
                        ((piece.PieceColor == GameColor.BLACK) && (piece.Y == 7)))
                    {
                        PromotePawn(piece as Pawn);
                    }
                }
                return true;
            }
            else
                return false; 
        }

        public Piece KillPiece(Square square)
        {
            Piece killedPiece = square.PieceContained;
            if (killedPiece != null)
            {
                _squares[square.X, square.Y].PieceContained = null;

                if (killedPiece.PieceId == "WK")
                    RemoveKing(GameColor.WHITE);
                if (killedPiece.PieceId == "BK")
                    RemoveKing(GameColor.BLACK);
            }


            return killedPiece;
        }

        public List<Piece> GetPiecesFromSide(GameColor color)
        {
            List<Piece> pieces = new List<Piece>();
            foreach (Piece p in GetRemainingPieces())
            {
                if (p.PieceColor == color)
                    pieces.Add(p);
            }
            return pieces;
        }

        public List<Move> GetPossibleMovesFromSide(GameColor color)
        {
            List<Move> moves = new List<Move>();
            foreach (Piece p in GetPiecesFromSide(color))
            {
                moves.AddRange(p.PossibleMoves(this));
            }
            return moves;
        }

        public List<Move> GetAttackMovesFromSide(GameColor color)
        {
            List<Move> moves = new List<Move>();
            foreach (Move m in GetPossibleMovesFromSide(color))
            {
                if (m.IsAttack)
                    moves.Add(m);
            }

            return moves;
        }

        public List<Piece> GetRemainingPieces()
        {
            List<Piece> pieces = new List<Piece>();
            foreach (Square square in _squares)
            {
                if (square.IsOccupied)
                    pieces.Add(square.PieceContained);
            }
            return pieces;
        }

        /// <summary>
        /// Promote the pawn to queen when it reaches the end of the board
        /// </summary>
        /// <param name="pawn">The pawn</param>
        public void PromotePawn(Pawn pawn)
        {
            Square square = GetSquare(pawn.X, pawn.Y);

            KillPiece(square);
            Piece queen = PieceFactory.GetQueen(pawn.PieceId[0] + "Q_P", pawn.PieceColor);
            SetPiece(queen, square);
        }


        public Square GetSquare(int x, int y)
        {
            return _squares[x, y];
        }
        public int Width
        {
            get => COLUMNS;
        }
        public int Height
        {
            get => ROWS;
        }
        public Square[,] BoardSquares
        {
            get => _squares;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
