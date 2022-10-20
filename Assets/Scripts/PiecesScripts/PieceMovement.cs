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
        AfterImage = 8, // 00001000 // This move left an afterimage
        Castle = 16, // 00010000
        EnPassant = 32, // 00100000 // This move landed on an afterimage
        PromotionRank = 64, // 01000000  // This move landed on the promotion rank
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
        public void GenerateMovesToList(ref List<Move> pieceMoves) {
            BoardPosition startPosition = GameManager.Instance.gameBoard.TransformIntoBoardPosition(this.transform);
            GenerateMovesToListImplementation(ref pieceMoves, startPosition);
        }
        public abstract void GenerateMovesToListImplementation(ref List<Move> pieceMoves, BoardPosition startPosition);

        public void MovePiece() {
            firstMove = false;
        }

        protected void GenSlidingStraightMoves(ref List<Move> moves, BoardPosition startPosition, int axisIndex, bool positive = true, bool negative = true, int maxRange = Int32.MaxValue) {
            int start_axis_val = startPosition[axisIndex];
            if(positive) LoopMoves(ref moves, start_axis_val, 1);
            if(negative) LoopMoves(ref moves, start_axis_val, -1);

            void LoopMoves(ref List<Move> moves, int start_axis_val, int increment_val) {
                for (int i = start_axis_val + increment_val, n = 0; i < GameManager.Instance.gameBoard.boardBoundaries[axisIndex] && i >= 0 && n < maxRange; i += increment_val, n++) {
                    BoardPosition end_pos = new BoardPosition(startPosition);
                    end_pos.SetValue(axisIndex, i);
                    Move move = new Move(startPosition, end_pos);

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

        protected void GenSlidingDiagonalMoves(ref List<Move> moves, BoardPosition startPosition, int axisIndex_1, int axisIndex_2, bool diagonal = true, bool antiDiagonal = true, int maxRange = Int32.MaxValue) {
            int start_axis_val_1 = startPosition[axisIndex_1];
            int start_axis_val_2 = startPosition[axisIndex_2];
            if(diagonal) {
                LoopMoves(ref moves, start_axis_val_1, start_axis_val_2, 1, 1);
                LoopMoves(ref moves, start_axis_val_1, start_axis_val_2, -1, -1);
            }
            if(antiDiagonal) {
                LoopMoves(ref moves, start_axis_val_1, start_axis_val_2, 1, -1);
                LoopMoves(ref moves, start_axis_val_1, start_axis_val_2, -1, 1);
            }

            void LoopMoves(ref List<Move> moves, int startAxisVal_1, int startAxisVal_2, int incrementVal_1, int incrementVal_2) {
                for (int i = startAxisVal_1 + incrementVal_1, j = startAxisVal_2 + incrementVal_2, n = 0;
                    i < GameManager.Instance.gameBoard.boardBoundaries[axisIndex_1] && i >= 0 && j < GameManager.Instance.gameBoard.boardBoundaries[axisIndex_2] && j >= 0 & n < maxRange;
                    i += incrementVal_1, j += incrementVal_2, n++)
                {
                    BoardPosition endPosition = new BoardPosition(startPosition);
                    endPosition.SetValue(axisIndex_1, i);
                    endPosition.SetValue(axisIndex_2, j);
                    Move move = new Move(startPosition, endPosition);

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