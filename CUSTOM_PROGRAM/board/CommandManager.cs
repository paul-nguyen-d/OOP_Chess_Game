using CUSTOM_PROGRAM.pieces;
using System;
using System.Collections.Generic;

namespace CUSTOM_PROGRAM.board
{
    /// <summary>
    /// It will manage all of the commands in the program.
    /// It will check if a given move is attack or not and it will execute the corresponding command.
    /// CommandManager also store a stack of command to undo and redo when the user needs
    /// </summary>
    public class CommandManager
    {
        /// <summary>
        /// Command to place a piece on the board. Or basic move without attacking
        /// </summary>
        private class PlacePieceCommand : ICommand
        {
            private Move _move;
            private IBoard _board;
            
            /// <summary>
            /// Create the command
            /// </summary>
            /// <param name="board">Current state of the board</param>
            /// <param name="move">The move</param>
            public PlacePieceCommand(IBoard board, Move move)
            {
                _board = board;
                _move = move;
            }

            /// <summary>
            /// Call the board to move the piece with the given move
            /// </summary>
            /// <returns>True if successful, else otherwise</returns>
            public bool Execute()
            {
                return _board.MovePiece(_move);
            }

            /// <summary>
            /// It will move the piece back with the inverse move of the given move
            /// </summary>
            public void Undo()
            {
                Move inverseMove = _move.InverseMove();

                _board.MoveWithoutCheck(inverseMove);
            }

            /// <summary>
            /// Move the piece again but the move doesn't need to be validated
            /// </summary>
            public void Redo()
            {
                _board.MoveWithoutCheck(_move);
            }
        }

        private class AttackPieceCommand : ICommand
        {
            private Move _move;
            private IBoard _board;
            private Piece _killedPiece;

            /// <summary>
            /// Create the command
            /// </summary>
            /// <param name="board">Current state of the board</param>
            /// <param name="move">The move</param>
            public AttackPieceCommand(IBoard board, Move move)
            {
                _board = board;
                _move = new Move(move.StartSquare, move.DestSquare, move.AttackedSquare);
                _killedPiece = _board.GetPiece(_move.AttackedSquare);
            }

            /// <summary>
            /// Call the board to move the piece with the given move
            /// </summary>
            /// <returns>True if successful, else otherwise</returns>
            public bool Execute()
            {
                return _board.MovePiece(_move);
            }

            /// <summary>
            /// It will move the piece back with the inverse move of the given move.
            /// It will place back the killed piece to its original square
            /// </summary>
            public void Undo()
            {
                Move inverseMove = _move.InverseMove();

                _board.MoveWithoutCheck(inverseMove);
                _board.SetPiece(_killedPiece, _move.AttackedSquare);
            }

            /// <summary>
            /// Move the piece again but the move doesn't need to be validated
            /// </summary>
            public void Redo()
            {
                _board.MoveWithoutCheck(_move); 
            }
        }

        //private class PawnPromotionCommand : ICommand
        //{
        //    private ChessBoard _board;
        //    private Pawn? _pawn = null;

        //    public PawnPromotionCommand(ChessBoard board, Move move)
        //    {
        //        _board = board;

        //        Piece piece = _board.GetPiece(move.StartSquare);
        //        if (piece.PieceId[1] == 'P')
        //            _pawn = piece as Pawn;
        //    }

        //    public bool Execute()
        //    {
        //        if (_pawn != null)
        //        {
        //            if (((_pawn.PieceColor == GameColor.WHITE) && (_pawn.Y == 0)) ||
        //                ((_pawn.PieceColor == GameColor.BLACK) && (_pawn.Y == 7)))
        //            {
        //                _board.PromotePawn(_pawn);
        //                return true;
        //            }
        //        }

        //        return false;
        //    }

        //    public void Undo()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public void Redo()
        //    {
        //        throw new NotImplementedException();
        //    }

        //}

        /// <summary>
        /// Store all of the execute commands
        /// </summary>
        private Stack<ICommand> _commandHistory;
        /// <summary>
        /// Store all of the undo-ed commands
        /// </summary>
        private Stack<ICommand> _commandRedo;
        public CommandManager()
        {
            _commandHistory = new Stack<ICommand>();
            _commandRedo = new Stack<ICommand>();
        }

        /// <summary>
        /// Select the command based on the given move and execute it
        /// </summary>
        /// <param name="board">The current state of the board</param>
        /// <param name="move">The move</param>
        /// <returns>True if success, false otherwise</returns>
        public bool ExecuteCommand(IBoard board, Move move)
        {
            ICommand command;
            bool result;
            if (move.IsAttack)
                command = new AttackPieceCommand(board, move);
            else
                command = new PlacePieceCommand(board, move);

            result = command.Execute();
            if (result)
                _commandHistory.Push(command);

            // if the player does a different move after undoing
            // clear all of the available redo commands
            if ((_commandRedo.Count > 0) && (command != _commandRedo.Peek()))
                _commandRedo.Clear();

            return result;
        }

        public void UndoCommand()
        {
            if (_commandHistory.Count > 0)
            {
                ICommand lastCommand = _commandHistory.Pop();
                lastCommand.Undo();

                _commandRedo.Push(lastCommand);
            }
        }

        public void RedoCommand()
        {
            if (_commandRedo.Count > 0)
            {
                ICommand lastUndo = _commandRedo.Pop();
                lastUndo.Redo();
                _commandHistory.Push(lastUndo);
            }
        }

    }
}
