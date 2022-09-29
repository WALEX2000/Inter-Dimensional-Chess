namespace Chess.Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Chess.Pieces;
    using Chess.Board;
    using System;

    public class GameManager : MonoBehaviour
    {
        // Singleton
        private static GameManager _instance;
        public static GameManager Instance { 
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<GameManager>();
                    if(_instance == null) {
                        throw new System.Exception("Trying to access Chess.Game.GameManager but it does not exist in the Scene!");
                    }
                }
                return _instance;
            } 
        }
        private void Awake() {
            _instance = this;
        }

        // Constants
        public const string GUIHolderObjectName = "GUIHolder";

        public ChessBoard gameBoard = new ChessBoard();
        public Transform boardTransform;

        // Game Function Methods
        public bool isWhiteTurn = true;
        public void StartTurn() {
            gameBoard.CalculateTeamMoves(isWhiteTurn);
        }

        public void EndTurn() {
            isWhiteTurn = !isWhiteTurn;
            StartTurn();
        }

        // Piece Selection
        public GameObject moveIndicatorPrefab;
        private ClickablePiece selectedPiece = null;
        public void SelectPiece(ClickablePiece piece) {
            DeselectPiece(); // deselect previous piece

            selectedPiece = piece;
            List<Move> possibleMoves = gameBoard.GetPieceMoves(piece.gameObject);

            foreach(Move move in possibleMoves) {
                BoardPosition indicatorPosition = move.endPosition;
                Transform guiTransform = boardTransform.GetChild(indicatorPosition.w).Find(GUIHolderObjectName);
                
                GameObject moveIndicator = GameObject.Instantiate(moveIndicatorPrefab, guiTransform);
                Vector3 indicatorPositionVector = new Vector3(indicatorPosition.x, indicatorPosition.y - 0.49f, indicatorPosition.z);
                moveIndicator.transform.localPosition = indicatorPositionVector;
                moveIndicator.GetComponent<MoveIndicator>().setMove(move);
                
                // set capture material if move is a capture
                GameObject capturedPiece = gameBoard.GetBoardElement(move.endPosition);
                if (gameBoard.IsElementBlack(capturedPiece) || gameBoard.IsElementWhite(capturedPiece)) {
                    moveIndicator.GetComponent<MoveIndicator>().setCaptureMat();
                }
            }
        }

        public void DeselectPiece() {
            if(selectedPiece is null) return;
            selectedPiece.Deselect();
            selectedPiece = null;
            DestroySelectionIndicators();
        }

        private void DestroySelectionIndicators() {
            foreach(Transform dimension in boardTransform) {
                Transform guiHolder = dimension.Find(GUIHolderObjectName);
                while(guiHolder.childCount > 0) {
                    DestroyImmediate(guiHolder.GetChild(0).gameObject);
                }
            }
        }
    }
}