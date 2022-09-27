namespace Chess.Pieces
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Chess.Game;
    using Chess.Board;

    public class PawnMoves : PieceMovement
    {
        public override void GenerateMovesToList(ref List<Move> moves)
        {
            BoardPosition start_pos = GameManager.Instance.GameBoard.TransformIntoBoardPosition(this.transform); // TODO This is not good practice
            GenForwardPawnMoves(ref moves, start_pos, 1); // y
            GenForwardPawnMoves(ref moves, start_pos, 1, -1); // -y
            GenForwardPawnMoves(ref moves, start_pos, 3); // w
            if(GameManager.Instance.GameBoard.IsElementWhite(this.gameObject)) {
                GenForwardPawnMoves(ref moves, start_pos, 2); // z
                GenPawnCaptureMoves(ref moves, start_pos, 2, 0); // z/x
            } else {
                GenForwardPawnMoves(ref moves, start_pos, 2, -1); // z
                GenPawnCaptureMoves(ref moves, start_pos, 2, 0, -1); // z/x
            }
            
            GenPawnCaptureMoves(ref moves, start_pos, 1, 0); // y/x
            GenPawnCaptureMoves(ref moves, start_pos, 1, 0, -1); // -y/x
            GenPawnCaptureMoves(ref moves, start_pos, 3, 0); // w/x
        }

        private void GenPawnCaptureMoves(ref List<Move> moves, BoardPosition start_pos, int forward_axis_index, int lateral_axis_index, int multiplier = 1)
        { // Go forward on axis_index and to the left/right on lateral axis
            int start_forward_axis_val = start_pos[forward_axis_index];
            BoardPosition end_pos_1 = new BoardPosition(start_pos);
            end_pos_1.setValue(forward_axis_index, start_forward_axis_val + 1 * multiplier); // Go forward on axis_index
            BoardPosition end_pos_2 = new BoardPosition(end_pos_1);
            int start_lateral_axis_val = start_pos[lateral_axis_index];
            end_pos_1.setValue(lateral_axis_index, start_lateral_axis_val + 1); // Go positive on lateral_axis_index
            end_pos_2.setValue(lateral_axis_index, start_lateral_axis_val - 1); // Go negative on lateral_axis_index
            Move move_1 = new Move(start_pos, end_pos_1);
            Move move_2 = new Move(start_pos, end_pos_2);

            MoveOutcome moveOutcome_1 = GameManager.Instance.GameBoard.CheckMoveRules(this.gameObject, move_1);
            if(moveOutcome_1 == MoveOutcome.Capture) {
                moves.Add(move_1);
            }

            MoveOutcome moveOutcome_2 = GameManager.Instance.GameBoard.CheckMoveRules(this.gameObject, move_2);
            if(moveOutcome_2 == MoveOutcome.Capture) {
                moves.Add(move_2);
            }
        }

        private void GenForwardPawnMoves(ref List<Move> moves, BoardPosition start_pos, int axis_index, int multiplier = 1)
        {
            int start_axis_val = start_pos[axis_index];
            BoardPosition end_pos = new BoardPosition(start_pos);
            end_pos.setValue(axis_index, start_axis_val + 1 * multiplier);
            Move move = new Move(start_pos, end_pos);
            MoveOutcome moveOutcome = GameManager.Instance.GameBoard.CheckMoveRules(this.gameObject, move);
            if (moveOutcome is MoveOutcome.Valid) moves.Add(move);
            else return;

            if (firstMove) {
                end_pos.setValue(axis_index, start_axis_val + 2 * multiplier);
                move = new Move(start_pos, end_pos);
                moveOutcome = GameManager.Instance.GameBoard.CheckMoveRules(this.gameObject, move);
                if (moveOutcome is MoveOutcome.Valid) moves.Add(move);
                else return;
            }
        }
    }
}
