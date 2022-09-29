namespace Chess.Pieces
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Chess.Game;
    using Chess.Board;

    public class KnightMoves : PieceMovement
    {
        private BoardPosition[] possibleMoves = new BoardPosition[] {
            new BoardPosition(1, 2, 0, 0),
            new BoardPosition(1, -2, 0, 0),
            new BoardPosition(1, 0, 2, 0),
            new BoardPosition(1, 0, -2, 0),
            new BoardPosition(1, 0, 0, 2),
            new BoardPosition(1, 0, 0, -2),
            new BoardPosition(-1, 2, 0, 0),
            new BoardPosition(-1, -2, 0, 0),
            new BoardPosition(-1, 0, 2, 0),
            new BoardPosition(-1, 0, -2, 0),
            new BoardPosition(-1, 0, 0, 2),
            new BoardPosition(-1, 0, 0, -2),
            new BoardPosition(2, 1, 0, 0),
            new BoardPosition(2, -1, 0, 0),
            new BoardPosition(2, 0, 1, 0),
            new BoardPosition(2, 0, -1, 0),
            new BoardPosition(2, 0, 0, 1),
            new BoardPosition(2, 0, 0, -1),
            new BoardPosition(-2, 1, 0, 0),
            new BoardPosition(-2, -1, 0, 0),
            new BoardPosition(-2, 0, 1, 0),
            new BoardPosition(-2, 0, -1, 0),
            new BoardPosition(-2, 0, 0, 1),
            new BoardPosition(-2, 0, 0, -1),
            new BoardPosition(0, 1, 2, 0),
            new BoardPosition(0, 1, -2, 0),
            new BoardPosition(0, 1, 0, 2),
            new BoardPosition(0, 1, 0, -2),
            new BoardPosition(0, -1, 2, 0),
            new BoardPosition(0, -1, -2, 0),
            new BoardPosition(0, -1, 0, 2),
            new BoardPosition(0, -1, 0, -2),
            new BoardPosition(0, 2, 1, 0),
            new BoardPosition(0, 2, -1, 0),
            new BoardPosition(0, 2, 0, 1),
            new BoardPosition(0, 2, 0, -1),
            new BoardPosition(0, -2, 1, 0),
            new BoardPosition(0, -2, -1, 0),
            new BoardPosition(0, -2, 0, 1),
            new BoardPosition(0, -2, 0, -1),
            new BoardPosition(0, 0, 1, 2),
            new BoardPosition(0, 0, 1, -2),
            new BoardPosition(0, 0, -1, 2),
            new BoardPosition(0, 0, -1, -2),
            new BoardPosition(0, 0, 2, 1),
            new BoardPosition(0, 0, 2, -1),
            new BoardPosition(0, 0, -2, 1),
            new BoardPosition(0, 0, -2, -1),
        };
        public override void GenerateMovesToListImplementation(ref List<Move> moves, BoardPosition startPosition)
        {
            foreach (BoardPosition possible_pos in possibleMoves)
            {
                BoardPosition newPosition = startPosition.Add(possible_pos);
                Move move = new Move(startPosition, newPosition);
                GameManager.Instance.gameBoard.CheckMoveOutcome(this.gameObject, ref move);
                if (move.outcome == MoveOutcome.BasicMove || move.outcome == MoveOutcome.Capture)
                    moves.Add(move);
            }
        }
    }
}
