using CUSTOM_PROGRAM.board;
using CUSTOM_PROGRAM.pieces;
using CUSTOM_PROGRAM.player;
using CUSTOM_PROGRAM.view;

namespace CUSTOM_PROGRAM.core
{
    /// <summary>
    /// ChessManager manages the board, gets the input from the user, process it and display the game.
    /// It also processes players' turns.
    /// </summary>
    public class ChessManager
    {
        private CommandManager _commandManager;
        private GameColor _turn;
        private ChessBoard _board;
        private IDisplayStrategy _displayStrategy;
        private bool playing = true;

        private Player[] _players;


        public ChessManager()
        {
            _commandManager = new CommandManager();
            _turn = GameColor.WHITE;
            _board = new ChessBoard();
            _displayStrategy = new ChessGUI(_board);
            _players = new Player[2];
            InitializePlayer();
        }

        /// <summary>
        /// Run the game
        /// </summary>
        public void Execute()
        {
            _displayStrategy.Display(_board, _players);
            while (playing)
            {
                ProcessInput();
                _displayStrategy.Display(_board, _players);
                Update();
            }
            _displayStrategy.Destroy();
        }

        /// <summary>
        /// Process the input from the user
        /// </summary>
        private void ProcessInput()
        {
            string input = _displayStrategy.GameInput();
            if (input != "")
            {
                //Console.WriteLine($"\nThe input is {input}\n");

                if (input == "quit")
                    playing = false;

                else if (input == "undo")
                {
                    _commandManager.UndoCommand();
                    ChangeTurn();
                }
                else if (input == "redo")
                {
                    _commandManager.RedoCommand();
                    ChangeTurn();
                }
                else if (input == "change display" || input == "gui")
                    ChangeDisplay();
                else
                    MakeMove(input);
            }
        }

        /// <summary>
        /// Execute the move based on the user's input
        /// </summary>
        /// <param name="input">Input from user</param>
        private void MakeMove(string input)
        {
            string[] splitInput = input.Split(" ",  StringSplitOptions.RemoveEmptyEntries);
            Piece movedPiece = null;
            foreach (Piece piece in _board.GetRemainingPieces())
            {
                if (piece.PieceId.ToLower() == splitInput[0])
                {
                    movedPiece = piece;
                    break;
                }
            }

            if (movedPiece != null)
            {
                if (movedPiece.PieceColor == _turn)
                {
                    Square startSquare = _board.GetSquare(movedPiece.X, movedPiece.Y);
                    Square destSquare = _board.GetSquare(Int32.Parse(splitInput[1]) - 1, Int32.Parse(splitInput[2]) - 1);

                    if (startSquare != destSquare)
                    {
                        Move move = new Move(startSquare, destSquare);
                        foreach (Move m in movedPiece.PossibleMoves(_board))
                        {
                            if (m.Equals(move))
                            {
                                move = m;
                                break;
                            }
                        }
                        Console.WriteLine(move.ToString());
                        bool result = _commandManager.ExecuteCommand(_board, move);
                        if (result)
                            ChangeTurn();
                        else
                        {
                            Console.WriteLine("Invalid Move");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Piece needs to move to a different square");
                    }
                }
                else
                {
                    Console.WriteLine("Not your turn!");
                }
            }
        }

        /// <summary>
        /// Create new players and start their timers
        /// </summary>
        private void InitializePlayer()
        {
            _players[0] = new Player("White player", GameColor.WHITE);
            _players[1] = new Player("Black player", GameColor.BLACK);
            _players[0].StartTimer();
        }

        /// <summary>
        /// Check if any of the players has won
        /// </summary>
        /// <returns>True if a player has won</returns>
        private bool CheckIfPlayerWin()
        {
            // if a player's king has been eaten
            if (_board.GetKing(GameColor.BLACK) == null)
            {
                _players[0].HasWon = true;
                return true;
            }
            else if (_board.GetKing(GameColor.WHITE) == null)
            {
                _players[1].HasWon = true;
                return true;
            }

            // a player's automatically lost if its timer runs out
            else
            {
                for (int i=0;i< _players.Length; i++)
                {
                    if (_players[i].RemainingTime == "00:00")
                    {
                        _players[i].PauseTimer();
                        _players[1 - i].HasWon = true;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Update the game
        /// </summary>
        private void Update()
        {
            UpdateGameOver();
        }

        private void UpdateGameOver()
        {
            if (CheckIfPlayerWin())
            {
                _players[0].PauseTimer();
                _players[1].PauseTimer();
                _displayStrategy.GameIsOver();
            }
        }

        /// <summary>
        /// Change the turn after a player has made a move
        /// </summary>
        private void ChangeTurn()
        {
            if (_turn == GameColor.WHITE)
            {
                _turn = GameColor.BLACK;
                _players[0].PauseTimer();
                _players[1].StartTimer();

            }
            else
            {
                _turn = GameColor.WHITE;
                _players[0].StartTimer();
                _players[1].PauseTimer();
            }
        }

        /// <summary>
        /// Switch to a different display method
        /// </summary>
        private void ChangeDisplay()
        {
            _displayStrategy.Destroy();

            if (_displayStrategy.Type == DisplayType.CONSOLE)
                _displayStrategy = new ChessGUI(_board);
            else
                _displayStrategy = new ChessConsole(_board);
        }

    }
}
