using CUSTOM_PROGRAM.board;
using CUSTOM_PROGRAM.pieces;
using CUSTOM_PROGRAM.player;

namespace CUSTOM_PROGRAM.view
{
    /// <summary>
    /// Display the game to the terminal
    /// </summary>
    public class ChessConsole :  IDisplayStrategy
    {
        private ChessBoard _board;
        private string[] validCommands = { "undo", "redo", "gui", "change display", "quit" };
        private bool _gameOver;

        /// <summary>
        /// Create the termimal
        /// </summary>
        /// <param name="board">Current state of the board</param>
        public ChessConsole(ChessBoard board)
        {
            _board = board;
            _gameOver = false;
            Initialize();
        }

        /// <summary>
        /// Start writing to the terminal
        /// </summary>
        private void Initialize()
        {
            Console.Clear();
            Console.WriteLine("Initializing console game...");
            Console.WriteLine("-----------------------------\n");
        }
        public void Destroy()
        {
            Console.Clear();
        }

        public void Display(ChessBoard board, Player[] players)
        {
            _board = board;
            string line = "**12345678\n";
            line += "**--------\n";

            for (int i = 0; i < _board.BoardSquares.GetLength(0); i++)
            {
                line += i + 1;
                line += "|";
                for (int j = 0; j < _board.BoardSquares.GetLength(1); j++)
                {
                    Square square = _board.GetSquare(j, i);
                    Piece piece = _board.GetPiece(square);
                    if (piece != null)
                    {
                        if (piece.PieceColor == GameColor.WHITE)
                            line += char.ToLower(piece.PieceId[1]);
                        else
                            line += piece.PieceId[1];
                    }
                    else
                        line += "-";
                }
                line += "\n";
            }
            Console.WriteLine(line);

            // if game is over
            if (_gameOver)
                PrintGameOver(players);
        }

        public string GameInput()
        {
            if (!_gameOver)
            {
                string input = Console.ReadLine().Trim().ToLower();
                if (validCommands.Contains(input) || ValidateInput(input))
                    return input;
                else
                    Console.WriteLine("Invalid command\n");
            }
            else
                return "quit";

            return "";
        }

        /// <summary>
        /// Check if the input from the user is valid
        /// </summary>
        /// <param name="input">String input from the user</param>
        /// <returns></returns>
        private bool ValidateInput(string input)
        {
            string[] splitInput = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            // validate chosen piece
            Piece movedPiece = null;
            foreach (Piece piece in _board.GetRemainingPieces())
            {
                if (piece.PieceId.ToLower() == splitInput[0])
                {
                    movedPiece = piece;
                }
            }

            if (movedPiece == null)
            {
                Console.WriteLine("Invalid piece\n");
                return false;
            }

            // validate destination square
            try
            {
                if (!_board.CheckValidSquare(Int32.Parse(splitInput[1]) - 1, Int32.Parse(splitInput[2]))) {
                    Console.WriteLine("Invalid destination square\n");
                    return false;
                }
            }
            catch
            {
                Console.WriteLine("Coordinates are not numbers");
                return false;
            }

            return true;

        }

        public void GameIsOver()
        {
            _gameOver = true;
        }

        /// <summary>
        /// Print the name of the winning player to the terminal
        /// </summary>
        /// <param name="players">All the players</param>
        private void PrintGameOver(Player[] players)
        {
            foreach (Player p in players)
            {
                if (p.HasWon)
                {
                    Console.WriteLine($"{p.Name} WINS!");
                    Console.WriteLine("\nPress ENTER to quit");
                    Console.ReadLine();
                }
            }

        }

        public DisplayType Type { get => DisplayType.CONSOLE; }
    }
}
