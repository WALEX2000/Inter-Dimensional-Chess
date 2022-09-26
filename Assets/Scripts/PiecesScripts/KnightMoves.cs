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
        public override void GenerateMovesToList(ref List<Move> moves)
        {
            BoardPosition start_pos = GameManager.Instance.TransformIntoBoardPosition(this.transform); // TODO This is not good practice
            foreach (BoardPosition possible_pos in possibleMoves)
            {
                BoardPosition new_pos = start_pos.add(possible_pos);
                Move move = new Move(start_pos, new_pos);
                MoveOutcome moveOutcome = GameManager.Instance.CheckMoveRules(this.gameObject, move);
                if (moveOutcome == MoveOutcome.Valid || moveOutcome == MoveOutcome.Capture)
                    moves.Add(move);
            }
        }
    }
}
