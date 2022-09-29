namespace Chess.Pieces
{
    using UnityEngine;
    using System.Collections.Generic;
    using Chess.Board;
    using Chess.Game;
    using System;
    
    [Flags] public enum MoveOutcome {
        Invalid = 0, // 00000000
        BasicMove = 1, // 00000001
        Capture = 2, // 00000010
        FriendlyCapture = 4, // 00000100
        AfterImage = 8, // 00001000 This move left an afterimage
        Castle = 16, // 00010000
        EnPassant = 32, // 00100000
        Promotion = 64, // 01000000
    }

    public struct Move {
        public char? promotionType;
        public AfterImage afterImage;
        public MoveOutcome outcome;
        public BoardPosition startPosition;
        public BoardPosition endPosition;
        public Move(BoardPosition startPosition, BoardPosition endPosition, MoveOutcome outcome = MoveOutcome.Invalid, char? promotion = null, AfterImage afterImage = null) {
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.promotionType = promotion;
            this.afterImage = afterImage;
            this.outcome = outcome;
        }

        public int moveLength() {
            return startPosition.DistanceBetween(endPosition);
        }
    }

    public abstract class PieceMovement : MonoBehaviour
    {
        protected bool firstMove = true;
        public abstract void GenerateMovesToList(ref List<Move> pieceMoves);

        public void MovePiece() {
            firstMove = false;
        }

        protected void GenSlidingStraightMoves(ref List<Move> moves, BoardPosition start_pos, int axis_index, bool positive = true, bool negative = true, int maxRange = 100) { // TODO: Change 100 to something proper
            int start_axis_val = start_pos[axis_index];
            if(positive) LoopMoves(ref moves, start_axis_val, 1);
            if(negative) LoopMoves(ref moves, start_axis_val, -1);

            void LoopMoves(ref List<Move> moves, int start_axis_val, int increment_val) {
                for (int i = start_axis_val + increment_val, n = 0; i < 12 && i >= 0 && n < maxRange; i += increment_val, n++) { // TODO Change 12 to something more centralized
                    BoardPosition end_pos = new BoardPosition(start_pos);
                    end_pos.SetValue(axis_index, i);
                    Move move = new Move(start_pos, end_pos);

                    GameManager.Instance.gameBoard.CheckMoveOutcome(this.gameObject, ref move);
                    if ((move.outcome & MoveOutcome.BasicMove) != 0) {
                        moves.Add(move);
                        continue;
                    }
                    else if ((move.outcome & MoveOutcome.Capture) != 0) {
                        moves.Add(move);
                        break;
                    }
                    else {
                        break;
                    }
                }
            }
        }

        protected void GenSlidingDiagonalMoves(ref List<Move> moves, BoardPosition start_pos, int axis_index_1, int axis_index_2, bool diagonal = true, bool anti_diagonal = true, int maxRange = 100) {
            int start_axis_val_1 = start_pos[axis_index_1];
            int start_axis_val_2 = start_pos[axis_index_2];
            if(diagonal) {
                LoopMoves(ref moves, start_axis_val_1, start_axis_val_2, 1, 1);
                LoopMoves(ref moves, start_axis_val_1, start_axis_val_2, -1, -1);
            }
            if(anti_diagonal) {
                LoopMoves(ref moves, start_axis_val_1, start_axis_val_2, 1, -1);
                LoopMoves(ref moves, start_axis_val_1, start_axis_val_2, -1, 1);
            }

            void LoopMoves(ref List<Move> moves, int start_axis_val_1, int start_axis_val_2, int increment_val_1, int increment_val_2) {
                for (int i = start_axis_val_1 + increment_val_1, j = start_axis_val_2 + increment_val_2, n = 0; i < 12 && i >= 0 && j < 12 && j >= 0 & n < maxRange; i += increment_val_1, j += increment_val_2, n++) { // TODO Change 12 to something more centralized
                    BoardPosition end_pos = new BoardPosition(start_pos);
                    end_pos.SetValue(axis_index_1, i);
                    end_pos.SetValue(axis_index_2, j);
                    Move move = new Move(start_pos, end_pos);

                    GameManager.Instance.gameBoard.CheckMoveOutcome(this.gameObject, ref move);
                    if ((move.outcome & MoveOutcome.BasicMove) != 0) {
                        moves.Add(move);
                        continue;
                    }
                    else if ((move.outcome & MoveOutcome.Capture) != 0) {
                        moves.Add(move);
                        break;
                    }
                    else {
                        break;
                    }
                }
            }
        }
    }
}