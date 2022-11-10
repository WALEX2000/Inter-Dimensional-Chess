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

        public void SwitchFourthDimensionAxis(BoardAxis newAxis) {
            fourthDimensionAxis = newAxis;
            fourthDimensionValue = 0;
            if(selectedPiece is not null) {
                BoardPosition piecePosition = GameManager.Instance.gameBoard.GetPieceBoardPosition(selectedPiece.gameObject);
                fourthDimensionValue = piecePosition[((int)fourthDimensionAxis)];
            }
            DisplayBoard();
        }

        public void CycleFourthDimension(int value) {
            fourthDimensionValue+=value;
            int maxDimValue = GameManager.Instance.gameBoard.boardBoundaries[(int)fourthDimensionAxis];
            if(fourthDimensionValue >= maxDimValue) {
                fourthDimensionValue = 0;
            } else if (fourthDimensionValue < 0) {
                fourthDimensionValue = maxDimValue - 1;
            }
            DisplayBoard();
        }

        public void DisplayBoard() {
            if(selectedPiece is not null) {
                SelectPiece(selectedPiece);
            }
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

            // Display towers
            if(fourthDimensionAxis is BoardAxis.Y) return;
            GameObject[,,,] towerMatrix = GameManager.Instance.gameBoard.getTowerMatrix();
            for(int i = 0; i < towerMatrix.GetLength(firstDimensionIndex); i++) {
                for(int j = 0; j < towerMatrix.GetLength(secondDimensionIndex); j++) {
                    for(int k = 0; k < towerMatrix.GetLength(thirdimensionIndex); k++) {
                        position[firstDimensionIndex] = i;
                        position[secondDimensionIndex] = j;
                        position[thirdimensionIndex] = k;
                        GameObject tower = (GameObject) towerMatrix.GetValue(position);
                        if(tower != null) {
                            tower.transform.parent = this.transform;
                            tower.transform.localPosition = new Vector3(i, j-towerMatrix.GetLength(1), k);
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

            BoardPosition piecePosition = GameManager.Instance.gameBoard.GetPieceBoardPosition(piece.gameObject);
            if(piecePosition[(int)fourthDimensionAxis] != fourthDimensionValue) {
                return;
            }

            selectedPiece = piece;
            piece.ForceSelectMat();
            List<Move> possibleMoves = GameManager.Instance.gameBoard.GetPieceMoves(piece.gameObject);

            foreach(Move move in possibleMoves) {
                // check if indicatorPosition is in the current 4th dimension slice
                if(move.endPosition[(int)fourthDimensionAxis] != fourthDimensionValue) {
                    continue;
                }
                int firstDimensionIndex = (int)fourthDimensionAxis == 0 ? 3 : 0;
                int secondDimensionIndex = (int)fourthDimensionAxis == 1 ? 3 : 1;
                int thirdimensionIndex = (int)fourthDimensionAxis == 2 ? 3 : 2;
                
                GameObject moveIndicator = GameObject.Instantiate(moveIndicatorPrefab, MoveSelectionGUI);
                Vector3 indicatorPositionVector = new Vector3(move.endPosition[firstDimensionIndex], move.endPosition[secondDimensionIndex] + 0.001f, move.endPosition[thirdimensionIndex]);
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
