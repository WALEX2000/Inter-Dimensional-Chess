namespace Chess.Pieces
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Chess.Board;
    using Chess.Game;

    public class BishopMoves : PieceMovement
    {
        public override void GenerateMovesToList(ref List<Move> moves)
        {
            BoardPosition start_pos = GameManager.Instance.GameBoard.TransformIntoBoardPosition(this.transform); // TODO This is not good practice
            GenSlidingDiagonalMoves(ref moves, start_pos, 0,1); // x/y
            GenSlidingDiagonalMoves(ref moves, start_pos, 0,2); // x/z
            GenSlidingDiagonalMoves(ref moves, start_pos, 0,3); // x/w
            GenSlidingDiagonalMoves(ref moves, start_pos, 1,2); // y/z
            GenSlidingDiagonalMoves(ref moves, start_pos, 1,3); // y/w
            GenSlidingDiagonalMoves(ref moves, start_pos, 2,3); // z/w
        }
    }
}
