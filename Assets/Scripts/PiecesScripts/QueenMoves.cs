namespace Chess.Pieces
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Chess.Board;
    using Chess.Game;

    public class QueenMoves : PieceMovement
    {
        public override void GenerateMovesToListImplementation(ref List<Move> moves, BoardPosition startPosition)
        {
            GenSlidingStraightMoves(ref moves, startPosition, 0); // x
            GenSlidingStraightMoves(ref moves, startPosition, 1); // y
            GenSlidingStraightMoves(ref moves, startPosition, 2); // z
            GenSlidingStraightMoves(ref moves, startPosition, 3); // w

            GenSlidingDiagonalMoves(ref moves, startPosition, 0,1); // x/y
            GenSlidingDiagonalMoves(ref moves, startPosition, 0,2); // x/z
            GenSlidingDiagonalMoves(ref moves, startPosition, 0,3); // x/w
            GenSlidingDiagonalMoves(ref moves, startPosition, 1,2); // y/z
            GenSlidingDiagonalMoves(ref moves, startPosition, 1,3); // y/w
            GenSlidingDiagonalMoves(ref moves, startPosition, 2,3); // z/w
        }
    }
}
