namespace Chess.Pieces
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Chess.Board;
    using Chess.Game;

    public class RookMoves : PieceMovement
    {
        public override void GenerateMovesToListImplementation(ref List<Move> moves, BoardPosition startPosition)
        {
            GenSlidingStraightMoves(ref moves, startPosition, 0); // x
            GenSlidingStraightMoves(ref moves, startPosition, 1); // y
            GenSlidingStraightMoves(ref moves, startPosition, 2); // z
            GenSlidingStraightMoves(ref moves, startPosition, 3); // w
        }
    }
}
