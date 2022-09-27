namespace Chess.Pieces
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Chess.Board;
    using Chess.Game;

    public class MoveIndicator : MonoBehaviour {
        private void OnMouseDown()
        { // Click on MoveIndicator
            GameManager.Instance.GameBoard.MovePiece(transform);
        }
    }
}