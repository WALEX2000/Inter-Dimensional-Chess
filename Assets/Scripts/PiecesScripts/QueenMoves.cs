namespace Chess.Pieces
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Chess.Board;
    using Chess.Game;

    public class QueenMoves : PieceMovement
    {
        public override void GenerateMovesToList(ref List<Move> moves)
        {
            BoardPosition start_pos = GameManager.Instance.TransformIntoBoardPosition(this.transform); // TODO This is not good practice
            GenSlidingStraightMoves(ref moves, start_pos, 0); // x
            GenSlidingStraightMoves(ref moves, start_pos, 1); // y
            GenSlidingStraightMoves(ref moves, start_pos, 2); // z
            GenSlidingStraightMoves(ref moves, start_pos, 3); // w

            GenSlidingDiagonalMoves(ref moves, start_pos, 0,2); // x/z
            GenSlidingDiagonalMoves(ref moves, start_pos, 0,1); // x/y
            GenSlidingDiagonalMoves(ref moves, start_pos, 1,2); // y/z
            // TODO: Missing "w"
        }
    }
}
