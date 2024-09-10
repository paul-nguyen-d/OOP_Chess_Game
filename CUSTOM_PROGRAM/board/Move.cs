using CUSTOM_PROGRAM.pieces;
using System;
using System.Collections.Generic;

namespace CUSTOM_PROGRAM.board
{
    /// <summary>
    /// A move containing a starting square and destination square. 
    /// It may also contain an attacked square
    /// </summary>
    public class Move : IEquatable<Move>
    {
        private bool _isAttack;
        private Square _startSquare, _destSquare;
        private Square _attackedSquare;

        /// <summary>
        /// Basic move
        /// </summary>
        /// <param name="startSquare">Starting square</param>
        /// <param name="destSquare">Destination square</param>
        public Move(Square startSquare, Square destSquare)
        {
            _startSquare = startSquare;
            _destSquare = destSquare;
            _isAttack = false;
            _attackedSquare = null;
        }

        /// <summary>
        /// A move that attacks another piece
        /// </summary>
        /// <param name="startSquare">Starting square</param>
        /// <param name="destSquare">Destination square</param>
        /// <param name="attackedSquare">Square that will be attacked</param>
        public Move(Square startSquare, Square destSquare, Square attackedSquare)
        {
            _startSquare = startSquare;
            _destSquare = destSquare;
            _isAttack = true;
            _attackedSquare = attackedSquare;
        }

        /// <summary>
        /// Transform a basic move to attack move
        /// </summary>
        public void Attack()
        {
            _isAttack = true;
        }

        /// <summary>
        /// Return if a move is an attack move
        /// </summary>
        public bool IsAttack { get => _isAttack; }

        public Square AttackedSquare { get => _attackedSquare; }

        public Square StartSquare { get => _startSquare; }
        public Square DestSquare { get => _destSquare; }

        /// <summary>
        /// Return the inversion of a move.
        /// The starting square becomes the destination and vice versa
        /// </summary>
        /// <returns>The inverse move</returns>
        public Move InverseMove()
        {
            Move inverse = new Move(DestSquare, StartSquare);
            if (IsAttack)
                inverse.Attack();
            return inverse;
        }


        public bool Equals(Move other)
        {
            return this.StartSquare == other.StartSquare &&
                    this.DestSquare == other.DestSquare;
        }

        /// <summary>
        /// Transcribe the move with all of its attributes to string
        /// </summary>
        /// <returns>The move description</returns>
        public override string ToString()
        {
            return $"Start: (x: {_startSquare.X}, y: {_startSquare.Y}), Dest: (x: {_destSquare.X}, y: {_destSquare.Y}), isAttack: {_isAttack}";
        }
    }
}
