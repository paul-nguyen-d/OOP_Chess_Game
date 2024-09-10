using CUSTOM_PROGRAM.board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CUSTOM_PROGRAM.pieces
{
    /// <summary>
    /// PieceFactory create and return the pieces.
    /// This class is a Singleton class
    /// </summary>
    public class PieceFactory
    {
        private static PieceFactory instance = new PieceFactory();

        private PieceFactory() { }

        /// <param name="id">Piece's unique id</param>
        /// <param name="color">Piece's side</param>
        /// <returns>A pawn</returns>
        public static Piece GetPawn(string id, GameColor color)
        {
            Piece pawn =  new Pawn(id, color);
            return pawn;
        }
        
        /// <param name="id">Piece's unique id</param>
        /// <param name="color">Piece's side</param>
        /// <returns>A rook</returns>
        public static Piece GetRook(string id, GameColor color)
        {
            Piece rook = new Rook(id, color);
            return rook;
        }

        /// <param name="id">Piece's unique id</param>
        /// <param name="color">Piece's side</param>
        /// <returns>A knight</returns>
        public static Piece GetKnight(string id, GameColor color)
        {
            Piece knight = new Knight(id, color);
            return knight;
        }

        /// <param name="id">Piece's unique id</param>
        /// <param name="color">Piece's side</param>
        /// <returns>A bishop</returns>
        public static Piece GetBishop(string id, GameColor color)
        {
            Piece bishop = new Bishop(id, color);
            return bishop;
        }

        /// <param name="id">Piece's unique id</param>
        /// <param name="color">Piece's side</param>
        /// <returns>A queen</returns>
        public static Piece GetQueen(string id, GameColor color)
        {
            Piece queen = new Queen(id, color);
            return queen;
        }

        /// <param name="id">Piece's unique id</param>
        /// <param name="color">Piece's side</param>
        /// <returns>A king</returns>
        public static Piece GetKing(string id, GameColor color)
        {
            Piece king = new King(id, color);
            return king;
        }
    }
}
