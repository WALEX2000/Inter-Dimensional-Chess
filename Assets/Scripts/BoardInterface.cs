namespace Chess.Interface
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Chess.Game;
    using Chess.Board;
    using Chess.Pieces;
    using System;

    public class BoardInterface : MonoBehaviour
    {
        /* 
         * This class is responsible for displaying the board and pieces in the scene.
         * It is also responsible for handling user input and passing it to the GameManager.
        */
        public Transform boardInterfacePool;
        
        public BoardAxis fourthDimensionAxis = BoardAxis.W; // Value of the currently selected 4th dimension
        public int fourthDimensionValue = 2; // Value of the currently selected 4th dimension slice
        public bool display = false;
        private void Update() {
            if(display) {
                DisplayBoard();
                display = false;
            }
        }

        public void DisplayBoard() {
            DeselectPiece();
            while (transform.childCount > 0) {
                Transform child = transform.GetChild(0);
                child.parent = boardInterfacePool;
                child.localPosition = Vector3.zero;
            }
            
            GameObject[,,,] boardMatrix = GameManager.Instance.gameBoard.getBoardMatrix();
            int[] position = new int[4];
            position[(int)fourthDimensionAxis] = fourthDimensionValue;
            int firstDimensionIndex = (int)fourthDimensionAxis == 0 ? 3 : 0;
            int secondDimensionIndex = (int)fourthDimensionAxis == 1 ? 3 : 1;
            int thirdimensionIndex = (int)fourthDimensionAxis == 2 ? 3 : 2;

            for(int i = 0; i < boardMatrix.GetLength(firstDimensionIndex); i++) {
                for(int j = 0; j < boardMatrix.GetLength(secondDimensionIndex); j++) {
                    for(int k = 0; k < boardMatrix.GetLength(thirdimensionIndex); k++) {
                        position[firstDimensionIndex] = i;
                        position[secondDimensionIndex] = j;
                        position[thirdimensionIndex] = k;
                        GameObject piece = (GameObject) boardMatrix.GetValue(position);
                        if(piece != null) {
                            piece.transform.parent = this.transform;
                            piece.transform.localPosition = new Vector3(i, j, k);
                        }
                    }
                }
            }
        }

        // Piece Selection
        public GameObject moveIndicatorPrefab;
        public Transform MoveSelectionGUI;
        private ClickablePiece selectedPiece = null;
        public void SelectPiece(ClickablePiece piece) {
            DeselectPiece(); // deselect previous piece

            selectedPiece = piece;
            List<Move> possibleMoves = GameManager.Instance.gameBoard.GetPieceMoves(piece.gameObject);

            foreach(Move move in possibleMoves) {
                BoardPosition indicatorPosition = move.endPosition;

                // check if indicatorPosition is in the current 4th dimension slice
                if(indicatorPosition[(int)fourthDimensionAxis] != fourthDimensionValue) {
                    continue;
                }
                
                GameObject moveIndicator = GameObject.Instantiate(moveIndicatorPrefab, MoveSelectionGUI);
                Vector3 indicatorPositionVector = new Vector3(indicatorPosition.x, indicatorPosition.y + 0.001f, indicatorPosition.z);
                moveIndicator.transform.localPosition = indicatorPositionVector;
                moveIndicator.GetComponent<MoveIndicator>().setMove(move);
                
                if ((move.outcome & MoveOutcome.Capture) != 0) {
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
            while(MoveSelectionGUI.childCount > 0) {
                DestroyImmediate(MoveSelectionGUI.GetChild(0).gameObject);
            }
        }
    }
}
