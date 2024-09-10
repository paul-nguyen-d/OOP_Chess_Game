using CUSTOM_PROGRAM.board;
using CUSTOM_PROGRAM.player;
using System.Text;
using System.Threading.Tasks;

namespace CUSTOM_PROGRAM.view
{
    public interface IDisplayStrategy
    {
        /// <summary>
        /// Close the display window
        /// </summary>
        public void Destroy();

        /// <summary>
        /// Display the game
        /// </summary>
        /// <param name="board"></param>
        /// <param name="players"></param>
        public void Display(ChessBoard board, Player[] players);

        /// <summary>
        /// Get the input from the player and convert it to string
        /// </summary>
        /// <returns>The string input</returns>
        public string GameInput();

        /// <summary>
        /// Stop the game when one of the players has won
        /// </summary>
        public void GameIsOver(); 

        /// <summary>
        /// Display method
        /// </summary>
        public DisplayType Type { get; }
    }
}
