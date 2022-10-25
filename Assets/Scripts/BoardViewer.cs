namespace Chess.Viewer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Chess.Game;

    public class BoardViewer : MonoBehaviour
    {
        /* 
         * This class is responsible for displaying the board and pieces in the scene.
         * It is also responsible for handling user input and passing it to the GameManager.
        */
        public int extraDimensionValue = 0; // Value of the currently selected 4th dimension
        private int extraDimensionIndex = 3; // Index of the currently selected 4th dimension

        public void DisplayBoard() {
            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }
            
            GameObject[,,,] boardMatrix = GameManager.Instance.gameBoard.getBoardMatrix();

            for(int i = 0; i < boardMatrix.GetLength(0); i++) {
                for(int j = 0; j < boardMatrix.GetLength(1); j++) {
                    for(int k = 0; k < boardMatrix.GetLength(2); k++) {
                        if(boardMatrix[i,j,k,extraDimensionValue] != null) {
                            GameObject piece = boardMatrix[i,j,k,extraDimensionValue];
                            GameObject newPiece = GameObject.Instantiate(piece);
                            newPiece.transform.parent = this.transform;
                            newPiece.transform.position = new Vector3(i, j, k);
                        }
                    }
                }
            }
        }
    }
}
