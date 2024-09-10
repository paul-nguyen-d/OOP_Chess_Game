using CUSTOM_PROGRAM.board;
using SplashKitSDK;

namespace CUSTOM_PROGRAM.view
{
    /// <summary>
    /// A GUI square panel. Each panel contains a Square
    /// </summary>
    public class SquarePanel
    {
        private readonly int WIDTH = 70;
        private int _x, _y;
        private Square _square;
        private Rectangle _squareGrid;
        private Color _color;

        /// <summary>
        /// The state to highlight the panel
        /// </summary>
        public enum SquareState
        {
            CHOSEN,
            CHECKED,
            POSSIBLE_SQUARE
        }

        public SquarePanel(Square square)
        {
            _x = square.X * WIDTH + 10;
            _y = square.Y * WIDTH + 10;
            _square = square;

            _squareGrid = SplashKit.RectangleFrom(_x, _y, WIDTH, WIDTH);
            if (square.Color == "white")
                _color = Color.RGBColor(253, 205, 156);
            else
                _color = Color.RGBColor(145, 95, 46);
        }

        /// <summary>
        /// Draw the panel
        /// </summary>
        public void Draw()
        {
            SplashKit.FillRectangle(_color, _squareGrid);
        }

        /// <summary>
        /// Highlight the panel with the given state
        /// </summary>
        /// <param name="state">The state</param>
        public void Highlight(SquareState state)
        {
            switch (state)
            {
                // the panel will become yellow when a piece is chosen
                case SquareState.CHOSEN:
                    SplashKit.FillRectangle(Color.Yellow, _squareGrid);
                    break;

                // the panel will be red when the king is under check
                case SquareState.CHECKED:
                    SplashKit.FillRectangle(Color.Red, _squareGrid);
                    break;

                // each square that can be moved to by the piece will be marked with a green circle
                case SquareState.POSSIBLE_SQUARE:
                    Point2D squareCenter = SplashKit.RectangleCenter(_squareGrid);
                    SplashKit.FillCircle(Color.Green, squareCenter.X, squareCenter.Y, 10);
                    break;
            }
        }

        /// <summary>
        /// A panel that is hovered on by the mouse will be highlighted with a white grid around
        /// </summary>
        public void HoverOn()
        {
            SplashKit.DrawRectangle(Color.White, _squareGrid, SplashKit.OptionLineWidth(20));
        }

        /// <summary>
        /// Check whether the mouse is above a panel
        /// </summary>
        /// <param name="mousePos">Position of th mouse</param>
        /// <returns>True if there is a square under the mouse</returns>
        public bool AtMousePosition(Point2D mousePos)
        {
            return SplashKit.PointInRectangle(mousePos, _squareGrid);
        }

        /// <summary>
        /// Get the square of the panel
        /// </summary>
        public Square Square { get => _square; }
        public int X { get => _x; }
        public int Y { get => _y; }

    }
}
