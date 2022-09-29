namespace Chess.Pieces
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Chess.Board;
    using Chess.Game;

    public class KingMoves : PieceMovement
    {
        public override void GenerateMovesToList(ref List<Move> moves)
        {
            BoardPosition start_pos = GameManager.Instance.gameBoard.TransformIntoBoardPosition(this.transform); // TODO This is not good practice
            GenSlidingStraightMoves(ref moves, start_pos, 0, maxRange: 1); // x
            GenSlidingStraightMoves(ref moves, start_pos, 1, maxRange: 1); // y
            GenSlidingStraightMoves(ref moves, start_pos, 2, maxRange: 1); // z
            GenSlidingStraightMoves(ref moves, start_pos, 3, maxRange: 1); // w

            GenSlidingDiagonalMoves(ref moves, start_pos, 0,1, maxRange: 1); // x/y
            GenSlidingDiagonalMoves(ref moves, start_pos, 0,2, maxRange: 1); // x/z
            GenSlidingDiagonalMoves(ref moves, start_pos, 0,3, maxRange: 1); // x/w
            GenSlidingDiagonalMoves(ref moves, start_pos, 1,2, maxRange: 1); // y/z
            GenSlidingDiagonalMoves(ref moves, start_pos, 1,3, maxRange: 1); // y/w
            GenSlidingDiagonalMoves(ref moves, start_pos, 2,3, maxRange: 1); // z/w

            // TODO: Missing Castle Moves
            if(firstMove) {
                // try castle on x axis
                GenCastleMoves(ref moves, start_pos, 0);
                // try castle on w axis
                // try castle on y axis
            }
        }

        private void GenCastleMoves(ref List<Move> moves, BoardPosition start_pos, int axis_index) {
            // Check if the king is in check on its current position and the next two positions
            int start_axis_val = start_pos[axis_index];
            Move startMove = new Move(start_pos, start_pos);
            // TODO: Need to know how to find checks first
            BoardPosition check_pos = new BoardPosition(start_pos);

            for (int i = 0; i <= 2; i++) {
                check_pos.SetValue(axis_index, start_axis_val + i);
                Move move = new Move(start_pos, check_pos);
                GameManager.Instance.gameBoard.CheckMoveOutcome(gameObject, ref move);
                // TODO: Implement Castle
            }

            // Check if the rook is in the correct position

        }
    }
}
