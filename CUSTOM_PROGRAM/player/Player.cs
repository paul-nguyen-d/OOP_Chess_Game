
using CUSTOM_PROGRAM.board;
using CUSTOM_PROGRAM.pieces;
using System.Diagnostics;

namespace CUSTOM_PROGRAM.player
{
    /// <summary>
    /// A player in the game. It contains a name, color, countdown timer and it knows whether it has won
    /// </summary>
    public class Player
    {
        private string _name;
        private GameColor _color;
        private Stopwatch _timer;
        private bool _win;

        /// <summary>
        /// Create the player
        /// </summary>
        /// <param name="name">Name of the player</param>
        /// <param name="color">Side of the player</param>
        public Player(string name, GameColor color)
        {
            _name = name;
            _color = color;
            _timer = new Stopwatch();
            _win = false;
        }

        public string Name { get => _name; }
        public GameColor PlayerColor { get => _color; }

        /// <summary>
        /// Start the countdown timer
        /// </summary>
        public void StartTimer()
        {
            _timer.Start();
        }

        /// <summary>
        /// Pause ot stop the countdown timer
        /// </summary>
        public void PauseTimer()
        {
            _timer.Stop();
        }

        /// <summary>
        /// Get the remaing time and parse it to string
        /// </summary>
        public string RemainingTime
        {
            get
            {
                // countdown 30 minutes for each player
                TimeSpan t = TimeSpan.FromMinutes(30);
                t = t.Subtract(_timer.Elapsed);

                return t.ToString(@"mm\:ss");
            }
        }

        /// <summary>
        /// Check to see if the player's king is under check by another piece
        /// </summary>
        /// <param name="board">Current state of the board</param>
        /// <returns>True if the king is under check</returns>
        public bool HasKingInCheck(ChessBoard board)
        {
            if (board.GetKing(GameColor.BLACK) != null && board.GetKing(GameColor.WHITE) != null)
            {
                King king = board.GetKing(_color);

                List<Move> opponentAttackMoves;
                if (_color == GameColor.WHITE)
                {
                    opponentAttackMoves = board.GetAttackMovesFromSide(GameColor.BLACK);
                }
                else
                {
                    opponentAttackMoves = board.GetAttackMovesFromSide(GameColor.WHITE);
                }

                foreach (Move m in opponentAttackMoves)
                {
                    if (m.AttackedSquare == board.GetSquare(king.X, king.Y))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Get whether the player has won
        /// </summary>
        public bool HasWon { get => _win; set => _win = value; }
    }
}
