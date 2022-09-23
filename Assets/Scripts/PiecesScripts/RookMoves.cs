namespace Chess.Pieces
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Chess.Board;
    using Chess.Game;

    public class RookMoves : MonoBehaviour, IMoveable
    {
        public void GenerateMovesToList(ref List<Move> moves)
        {
            BoardPosition start_pos = GameManager.Instance.TransformIntoBoardPosition(this.transform.position); // TODO This is not good practice
            GenSlidingStraightMoves(ref moves, start_pos, 0); // x
            GenSlidingStraightMoves(ref moves, start_pos, 1); // y
            GenSlidingStraightMoves(ref moves, start_pos, 2); // z
            GenSlidingStraightMoves(ref moves, start_pos, 3); // w
        }

        private bool CheckMove(Move move) {
            return true;
        }

        private void GenSlidingStraightMoves(ref List<Move> moves, BoardPosition start_pos, int axis_index, bool positive = true, bool negative = true) {
            int start_axis_val = start_pos[axis_index];
            if(positive) LoopMoves(ref moves, start_axis_val, 1);
            if(negative) LoopMoves(ref moves, start_axis_val, -1);

            void LoopMoves(ref List<Move> moves, int start_axis_val, int increment_val) {
                for (int i = start_axis_val + increment_val; i < 12 && i >= 0; i += increment_val) {
                    // Check if this move is ok
                    BoardPosition end_pos = new BoardPosition(start_pos);
                    end_pos.setValue(axis_index, i);
                    Move move = new Move(start_pos, end_pos);
                    if(GameManager.Instance.CheckMove(move)) {
                        moves.Add(move);
                    }
                    else {
                        break;
                    }
                }
            }
        }
    }
}
