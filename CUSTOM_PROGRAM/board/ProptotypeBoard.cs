using System.Collections.Generic;
using CUSTOM_PROGRAM.pieces;

namespace CUSTOM_PROGRAM.board
{
    public class PrototypeBoard : IBoard
    {
        private Square[,] _squares;
        private List<Piece> _pieces;
        private Dictionary<GameColor, King> _allKings;

        private const int ROWS = 8;
        private const int COLUMNS = 8;


        public PrototypeBoard()
        {
            _squares = new Square[ROWS, COLUMNS];
            _pieces = new List<Piece>();
            _pieces = new List<Piece>();
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
            King whiteKing = PieceFactory.GetKing("WK", GameColor.WHITE) as King;
            SetKing(whiteKing, _squares[0, 0]);
            //SetPiece(PieceFactory.GetPawn("WP1", GameColor.WHITE), _squares[3, 1]);

            SetPiece(PieceFactory.GetRook("BR", GameColor.BLACK), _squares[3, 1]);
            SetPiece(PieceFactory.GetRook("BR", GameColor.BLACK), _squares[1, 3]);

            SetPiece(PieceFactory.GetQueen("BQ", GameColor.BLACK), _squares[0, 2]);

            foreach (Square square in _squares)
            {
                if (square.PieceContained != null)
                    _pieces.Add(square.PieceContained);
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


        // Set the king of a side
        // If a king of this side already exists, it will be removed
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

        // Get the king of a side
        public King GetKing(GameColor color)
        {
            if (_allKings.ContainsKey(color))
                return _allKings[color];

            return null;
        }

        // Remove the king of a side from the board
        public King RemoveKing(GameColor color)
        {
            King king = GetKing(color);
            if (king != null)
            {
                Square kingSquare = GetSquare(king.X, king.Y);
                KillPiece(kingSquare);
                _allKings.Remove(color);
            }
            return king;
        }


        // Simple move, no validation of the actual move
        public void MoveWithoutCheck(Move move)
        {
            Square startSquare = move.StartSquare;
            Square destSquare = move.DestSquare;
            Piece movedPiece = GetPiece(startSquare);

            // let the vitim piece know it has been killed
            if (move.IsAttack)
                KillPiece(destSquare);

            SetPiece(movedPiece, destSquare);

            _squares[startSquare.X, startSquare.Y].PieceContained = null;
        }

        // Move a piece on the board if the move is valid
        public bool MovePiece(Move move)
        {
            Square startSquare = move.StartSquare;
            List<Move> possibleMoves = startSquare.PieceContained.PossibleMoves(this);
            if (possibleMoves.Contains(move))
            {
                MoveWithoutCheck(move);
                return true;
            }
            else
                return false;
        }
        //Remove the piece from the board if the square has a piece
        public Piece KillPiece(Square square)
        {
            Piece killedPiece = square.PieceContained;
            if (killedPiece != null)
            {
                _squares[square.X, square.Y].PieceContained = null;
            }
            return killedPiece;
        }
        public List<Piece> GetPiecesFromSide(GameColor color)
        {
            List<Piece> pieces = new List<Piece>();
            foreach (Piece p in _pieces)
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

        // for testing 
        public void PrintBoard()
        {
            foreach (Square square in _squares)
            {
                Console.WriteLine($"Cell: row {square.Y}, col {square.X}, color: {square.Color}, piece: {square.PieceContained}");
            }
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
        public List<Piece> RemainingPieces
        {
            get
            {
                return _pieces;
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
