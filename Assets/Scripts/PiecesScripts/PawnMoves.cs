namespace Chess.Pieces
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Chess.Game;
    using Chess.Board;

    public class PawnMoves : PieceMovement
    {
        public override void GenerateMovesToListImplementation(ref List<Move> moves, BoardPosition startPosition)
        {
            GenForwardPawnMoves(ref moves, startPosition, 1); // y
            GenForwardPawnMoves(ref moves, startPosition, 1, -1); // -y
            GenForwardPawnMoves(ref moves, startPosition, 3); // w
            if(GameManager.Instance.gameBoard.IsElementWhite(this.gameObject)) {
                GenForwardPawnMoves(ref moves, startPosition, 2); // z
                GenPawnCaptureMoves(ref moves, startPosition, 2, 0); // z/x
            } else {
                GenForwardPawnMoves(ref moves, startPosition, 2, -1); // z
                GenPawnCaptureMoves(ref moves, startPosition, 2, 0, -1); // z/x
            }
            
            GenPawnCaptureMoves(ref moves, startPosition, 1, 0); // y/x
            GenPawnCaptureMoves(ref moves, startPosition, 1, 0, -1); // -y/x
            GenPawnCaptureMoves(ref moves, startPosition, 3, 0); // w/x
        }

        private void GenPawnCaptureMoves(ref List<Move> moves, BoardPosition start_pos, int forward_axis_index, int lateral_axis_index, int multiplier = 1)
        { // Go forward on axis_index and to the left/right on lateral axis
            int start_forward_axis_val = start_pos[forward_axis_index];
            BoardPosition end_pos_1 = new BoardPosition(start_pos);
            end_pos_1.SetValue(forward_axis_index, start_forward_axis_val + 1 * multiplier); // Go forward on axis_index
            BoardPosition end_pos_2 = new BoardPosition(end_pos_1);
            int start_lateral_axis_val = start_pos[lateral_axis_index];
            end_pos_1.SetValue(lateral_axis_index, start_lateral_axis_val + 1); // Go positive on lateral_axis_index
            end_pos_2.SetValue(lateral_axis_index, start_lateral_axis_val - 1); // Go negative on lateral_axis_index
            Move move_1 = new Move(start_pos, end_pos_1);
            Move move_2 = new Move(start_pos, end_pos_2);

            GameManager.Instance.gameBoard.CheckMoveOutcome(this.gameObject, ref move_1);
            if((move_1.outcome & MoveOutcome.Capture) != 0) { // Capture is an outcome
                moves.Add(move_1);
            } else if ((move_1.outcome & MoveOutcome.EnPassant) != 0)
            { // En Passant is an outcome
                move_1.outcome |= MoveOutcome.Capture;
                moves.Add(move_1);
            }

            GameManager.Instance.gameBoard.CheckMoveOutcome(this.gameObject, ref move_2);
            if((move_2.outcome & MoveOutcome.Capture) != 0) { // Capture is an outcome
                moves.Add(move_2);
            } else if ((move_2.outcome & MoveOutcome.EnPassant) != 0)
            { // En Passant is an outcome
                move_2.outcome |= MoveOutcome.Capture;
                moves.Add(move_2);
            }
        }

        private void GenForwardPawnMoves(ref List<Move> moves, BoardPosition start_pos, int axis_index, int multiplier = 1)
        {
            int start_axis_val = start_pos[axis_index];
            BoardPosition end_pos_1 = new BoardPosition(start_pos);
            end_pos_1.SetValue(axis_index, start_axis_val + 1 * multiplier);
            Move move = new Move(start_pos, end_pos_1);
            GameManager.Instance.gameBoard.CheckMoveOutcome(this.gameObject, ref move);
            if ((move.outcome & MoveOutcome.BasicMove) != 0) moves.Add(move);
            else return;

            if (firstMove) {
                BoardPosition end_pos_2 = new BoardPosition(start_pos);
                end_pos_2.SetValue(axis_index, start_axis_val + 2 * multiplier);
                AfterImage afterImage = new AfterImage(this.gameObject, end_pos_1);
                move = new Move(start_pos, end_pos_2, MoveOutcome.AfterImage, afterImage: afterImage);
                GameManager.Instance.gameBoard.CheckMoveOutcome(this.gameObject, ref move);
                if ((move.outcome & MoveOutcome.BasicMove) != 0) moves.Add(move);
                else return;
            }
        }
    }
}
