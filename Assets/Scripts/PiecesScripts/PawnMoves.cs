namespace Chess.Pieces
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Chess.Game;
    using Chess.Board;

    public class PawnMoves : MonoBehaviour, IMoveable
    {
        private bool FirstMove = true;
        public void GenerateMovesToList(ref List<Move> moves)
        {
            BoardPosition start_pos = GameManager.Instance.TransformIntoBoardPosition(this.transform); // TODO This is not good practice
            GenForwardPawnMoves(ref moves, start_pos, 1); // y
            GenForwardPawnMoves(ref moves, start_pos, 3); // w
            //if(GameManager.Instance.IsElementWhite(this.gameObject))
                GenForwardPawnMoves(ref moves, start_pos, 2); // z
            //else
                GenForwardPawnMoves(ref moves, start_pos, 2, -1); // z
        }

        private void GenForwardPawnMoves(ref List<Move> moves, BoardPosition start_pos, int axis_index, int multiplier = 1)
        {
            int start_axis_val = start_pos[axis_index];
            BoardPosition end_pos = new BoardPosition(start_pos);
            end_pos.setValue(axis_index, start_axis_val + 1 * multiplier);
            Move move = new Move(start_pos, end_pos);
            MoveOutcome moveOutcome = GameManager.Instance.CheckMoveRules(this.gameObject, move);
            if (moveOutcome is MoveOutcome.Invalid || moveOutcome is MoveOutcome.Capture) return;
            if (moveOutcome is MoveOutcome.Valid) moves.Add(move);

            if (FirstMove) {
                end_pos.setValue(axis_index, start_axis_val + 2 * multiplier);
                move = new Move(start_pos, end_pos);
                moveOutcome = GameManager.Instance.CheckMoveRules(this.gameObject, move);
                if (moveOutcome is MoveOutcome.Invalid || moveOutcome is MoveOutcome.Capture) return;
                if (moveOutcome is MoveOutcome.Valid) moves.Add(move);
            }
        }
    }
}
