using SplashKitSDK;
using CUSTOM_PROGRAM.board;
using CUSTOM_PROGRAM.pieces;
using CUSTOM_PROGRAM.player;
using System.Diagnostics;

namespace CUSTOM_PROGRAM.view
{
    /// <summary>
    /// Display the game with GUI. It contains 64 panels, representing 64 Squares in the board
    /// </summary>
    public class ChessGUI : IDisplayStrategy
    {
        private const int WIDTH = 900;
        private const int HEIGHT = 580;

        private SquarePanel[,] _squarePanels;
        private Piece? _chosenPiece = null;
        private ChessBoard _board;
        private Rectangle _button;
        private bool _gameOver;


        public ChessGUI(ChessBoard board)
        {
            _squarePanels = new SquarePanel[board.Width, board.Height];
            _board = board;
            _gameOver = false;
            Initialize();
        }

        /// <summary>
        /// Initilized all the panels, create a new window and load all fonts and sounds
        /// </summary>
        private void Initialize()
        {
            Console.WriteLine("Initializing GUI game...");

            for (int i = 0; i < _squarePanels.GetLength(0); i++)
            {
                for (int j = 0; j < _squarePanels.GetLength(1); j++)
                {
                    Square square = _board.GetSquare(i, j);
                    _squarePanels[i, j] = new SquarePanel(square);
                }
            }

            new Window("Chess Game", WIDTH, HEIGHT);

            SplashKit.LoadFont("libre", "LibreBaskerville-Regular.ttf");
            SplashKit.LoadFont("libreBold", "LibreBaskerville-Bold.ttf");
            SplashKit.LoadFont("montSemiBold", "Montserrat-SemiBold.ttf");
            SplashKit.LoadFont("mont_medium", "Montserrat-Medium.ttf");

            SplashKit.LoadSoundEffect("choose", "choose.wav");
            SplashKit.LoadSoundEffect("over", "win.wav");


            _button = SplashKit.RectangleFrom(650, HEIGHT/2-25, 177, 50);
        }


        public void Destroy()
        {
            SplashKit.CloseWindow("Chess Game");
        }

        public void Display(ChessBoard board, Player[] players)
        {
            _board = board;

            SplashKit.ProcessEvents();

            SplashKit.ClearScreen(Color.RGBColor(111, 94, 83));
            DrawBoard();
            DrawKingInCheck(players);

            DrawPieces();
            DrawButton();
            DrawPlayers(players);
            SoundEffect();

            if (_gameOver)
                DrawGameOver(players);
            SplashKit.RefreshScreen();
        }

        public string GameInput()
        {
            string input = "";

            if (SplashKit.WindowCloseRequested("Chess Game"))
                input = "quit";

            //if (_screen == GameScreen.WELCOME)
            //    input = WelcomeScreen();

            if (!_gameOver)
            {
                // undo
                if (SplashKit.KeyTyped(KeyCode.UKey))
                    input = "undo";

                // redo
                if (SplashKit.KeyTyped(KeyCode.RKey))
                    input = "redo";

                // change display
                if (SplashKit.KeyTyped(KeyCode.CKey))
                    input = "change display";


                Point2D mousePos = SplashKit.MousePosition();

                // select piece to move
                if (SplashKit.MouseClicked(MouseButton.LeftButton))
                {
                    if (SplashKit.PointInRectangle(mousePos, _button))
                        input = "change display";
                    else
                    {
                        try
                        {
                            if (_chosenPiece == null)
                            {
                                Piece piece = GetPieceAt(mousePos);
                                _chosenPiece = piece;
                            }
                            else
                            {
                                Square destSquare = GetSquareAt(mousePos);
                                input = $"{_chosenPiece.PieceId.ToLower()} {destSquare.X+1} {destSquare.Y+1}";
                                _chosenPiece = null;
                            }
                        }
                        catch (NullReferenceException ex)
                        {
                            Console.WriteLine("There is no square at that location");
                            _chosenPiece = null;
                        }
                    }
                }
            }

            return input;
        }

        //private string WelcomeScreen()
        //{
        //    SplashKit.LoadBitmap("bg", "bg1.jpg");
        //    SplashKit.DrawBitmap("bg", 0, 0);
        //    SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 60), 0, 0, WIDTH, HEIGHT);

        //    Point2D mousePos = SplashKit.MousePosition();

        //    Rectangle consoleGame = SplashKit.RectangleFrom(WIDTH/2-105, HEIGHT / 2 - 82, 259, 63);
        //    SplashKit.FillRectangle(Color.RGBColor(153, 88, 42), consoleGame);
        //    Rectangle GUIGame = SplashKit.RectangleFrom(WIDTH / 2 - 105, HEIGHT / 2 + 20, 259, 63);
        //    SplashKit.FillRectangle(Color.RGBColor(67, 40, 24), GUIGame);


        //    if (SplashKit.PointInRectangle(mousePos, consoleGame))
        //    {
        //        SplashKit.FillRectangle(Color.RGBColor(108, 88, 76), consoleGame);
        //        if (SplashKit.MouseClicked(MouseButton.LeftButton))
        //        {
        //            //_screen = GameScreen.GAME;
        //            return "change display";
        //        }

        //    }
        //    if (SplashKit.PointInRectangle(mousePos, GUIGame))
        //    {
        //        SplashKit.FillRectangle(Color.RGBColor(111, 29, 27), GUIGame);
        //        if (SplashKit.MouseClicked(MouseButton.LeftButton))
        //            _screen = GameScreen.GAME;
        //    }


        //    SplashKit.DrawText("Console Game", Color.WhiteSmoke, "libreBold", 30, WIDTH / 2 - 90, HEIGHT / 2 - 70);
        //    SplashKit.DrawText("GUI Game", Color.WhiteSmoke, "libreBold", 30, WIDTH / 2 - 60, HEIGHT / 2 + 32);

        //    return "";
        //}

        public DisplayType Type { get => DisplayType.GUI; }

        /// <summary>
        /// Draw the board to the window
        /// </summary>
        private void DrawBoard()
        {
            Point2D mousePos = SplashKit.MousePosition();

            // hover over square effect
            foreach (SquarePanel panel in _squarePanels)
            {
                panel.Draw();
                if (panel.AtMousePosition(mousePos) && !_gameOver)
                    panel.HoverOn();
            }

            if (_chosenPiece != null)
            {
                GetPanel(_chosenPiece.X, _chosenPiece.Y).Highlight(SquarePanel.SquareState.CHOSEN);
                DrawPossibleMoves(_chosenPiece);
            }
        }

        /// <summary>
        /// Draw all the pieces to the window
        /// </summary>
        private void DrawPieces()
        {
            foreach (Piece piece in _board.GetRemainingPieces())
            {
                Bitmap img = SplashKit.LoadBitmap(piece.PieceId, piece.ImagePath);
                SquarePanel panel = GetPanel(piece.X, piece.Y);
                SplashKit.DrawBitmap(img, panel.X + 4, panel.Y + 4);
            }
        }

        /// <summary>
        /// Display the names and remaining time of the players
        /// </summary>
        /// <param name="players"></param>
        private void DrawPlayers(Player[] players)
        {
            SplashKit.DrawText(players[1].RemainingTime, Color.WhiteSmoke, "montSemiBold", 40, 680, 120);
            SplashKit.DrawText($"{players[1].Name}", Color.WhiteSmoke, "libreBold", 25, 650, 70);

            SplashKit.DrawText(players[0].RemainingTime, Color.WhiteSmoke, "montSemiBold", 40, 680, 400);
            SplashKit.DrawText($"{players[0].Name}", Color.WhiteSmoke, "libreBold", 25, 645, 470);
        }

        /// <summary>
        /// Get the panel at the given coordinates
        /// </summary>
        /// <param name="x">X-coordinate</param>
        /// <param name="y">Y-coordinate</param>
        /// <returns>The panel</returns>
        private SquarePanel GetPanel(int x, int y)
        {
            for (int i = 0; i < _board.Height; i++)
            {
                for (int j = 0; j < _board.Width; j++)
                {
                    if ((i == x) && (j == y))
                        return _squarePanels[i, j];
                }
            }
            return null;
        }

        /// <summary>
        /// Return the piece at mouse position
        /// </summary>
        /// <param name="mousePos">Mouse position</param>
        /// <returns>The piece if found</returns>
        private Piece GetPieceAt(Point2D mousePos)
        {
            foreach (SquarePanel panel in _squarePanels)
            {
                if (panel.AtMousePosition(mousePos))
                {
                    try
                    {
                        Piece piece = _board.GetPiece(panel.Square);
                        Console.WriteLine(piece.ToString());
                        return piece;
                    }

                    catch (NullReferenceException ex)
                    {
                        Console.WriteLine("There is no piece at the chosen square");
                    }

                }
            }
            return null;
        }

        /// <summary>
        /// Return the square at mouse position
        /// </summary>
        /// <param name="mousePos">Mouse position</param>
        /// <returns>The square if found</returns>
        private Square GetSquareAt(Point2D mousePos)
        {
            foreach (SquarePanel panel in _squarePanels)
            {
                if (panel.AtMousePosition(mousePos))
                {
                    Square square = _board.GetSquare(panel.Square.X, panel.Square.Y);
                    return square;
                }
            }
            return null;
        }

        /// <summary>
        /// Highlight all of the possible squares the chosen piece can move to
        /// </summary>
        /// <param name="piece">The piece</param>
        private void DrawPossibleMoves(Piece piece)
        {
            List<Move> moves = piece.PossibleMoves(_board);
            foreach (Move move in moves)
            {
                Square destSquare = move.DestSquare;
                GetPanel(destSquare.X, destSquare.Y).Highlight(SquarePanel.SquareState.POSSIBLE_SQUARE);
            }
        }

        /// <summary>
        /// Highlight the square if the king is in check
        /// </summary>
        /// <param name="players">All the players</param>
        private void DrawKingInCheck(Player[] players)
        {
            foreach (Player p in players)
            {
                King king = _board.GetKing(p.PlayerColor);
                if (p.HasKingInCheck(_board))
                    GetPanel(king.X, king.Y).Highlight(SquarePanel.SquareState.CHECKED);
            }
        }

        /// <summary>
        /// Display the change display button
        /// </summary>
        private void DrawButton()
        {
            Point2D mousePos = SplashKit.MousePosition();

            SplashKit.FillRectangle(Color.RGBColor(153, 88, 42), _button);

            if (SplashKit.PointInRectangle(mousePos, _button))
                SplashKit.FillRectangle(Color.RGBColor(108, 88, 76), _button);

            SplashKit.DrawText("Console Game", Color.WhiteSmoke, "libreBold", 20, 662, HEIGHT/2-13);
        }

        /// <summary>
        /// Display the game over screen
        /// </summary>
        /// <param name="players"></param>
        private void DrawGameOver(Player[] players)
        {
            foreach (Player p in players)
            {
                if (p.HasWon)
                {
                    SplashKit.FillRectangle(SplashKit.RGBAColor(0, 0, 0, 0.6), 0, 0, WIDTH, HEIGHT);
                    SplashKit.DrawText($"{p.Name} WINS!", Color.WhiteSmoke, "libreBold", 50, 190, HEIGHT/2-40);
                }
            }
        }

        public void GameIsOver()
        {
            _gameOver = true;
        }

        /// <summary>
        /// Play the sound effect
        /// </summary>
        private void SoundEffect()
        {
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
                SplashKit.PlaySoundEffect("choose");
        }
    }
}
