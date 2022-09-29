namespace Chess.Pieces
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Chess.Board;
    using Chess.Game;

    public class MoveIndicator : MonoBehaviour {
        private Move move;
        public Material hoverMaterial;
        public Material idleMaterial;
        public Material captureMaterial;
        public Material captureHoverMaterial;
        public void setMove(Move move) {
            this.move = move;
        }

        public void setCaptureMat() {
            idleMaterial = captureMaterial;
            hoverMaterial = captureHoverMaterial;
            GetComponent<Renderer>().material = idleMaterial;
        }

        private void OnMouseDown()
        { // Click on MoveIndicator
            GameManager.Instance.gameBoard.MakeMove(move);
        }

        private void OnMouseEnter() {
            GetComponent<Renderer>().material = hoverMaterial;
        }

        private void OnMouseExit() {
            GetComponent<Renderer>().material = idleMaterial;
        }
    }
}