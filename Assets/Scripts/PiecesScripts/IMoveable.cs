namespace Chess.Pieces
{
    using System.Collections.Generic;
    using Chess.Board;

    public enum MoveOutcome {
        Valid,
        Invalid,
        Capture,
    }

    public struct Move {
        public BoardPosition start_position;
        public BoardPosition end_position;
        public Move(BoardPosition start_position, BoardPosition end_position) {
            this.start_position = start_position;
            this.end_position = end_position;
        }
    }
    public interface IMoveable
    {
        void GenerateMovesToList(ref List<Move> pieceMoves);
    }
}