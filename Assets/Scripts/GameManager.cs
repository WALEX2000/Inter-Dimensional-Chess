namespace Chess.Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Chess.Pieces;

    public class GameManager : MonoBehaviour
    {
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

        public bool IsWhiteTurn { get; set; } = true;

        private ClickablePiece selected_piece = null;
        public void SelectPiece(ClickablePiece piece) {
            if(selected_piece != null) {
                selected_piece.Deselect();
            }
            selected_piece = piece;
        }
    }    
}
