using CUSTOM_PROGRAM.board;
using CUSTOM_PROGRAM.pieces;
using CUSTOM_PROGRAM.view;
using CUSTOM_PROGRAM.player;
using CUSTOM_PROGRAM.core;

namespace CUSTOM_PROGRAM
{
    public class Program
    {
        public static void Main()
        {
            ChessManager game = new ChessManager();
            game.Execute();
        }
    }
}
