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

        // Game Functions
        private GameObject[,,,] boardMatrix = new GameObject[12, 12, 12, 6]; // Max Board Size (TODO Change to something more centralized)
        private Dictionary<GameObject, List<Move>> whitePieces = new Dictionary<GameObject, List<Move>>();
        private Dictionary<GameObject, List<Move>> blackPieces = new Dictionary<GameObject, List<Move>>();
        public bool isWhiteTurn = true;

        public void SetBoardElement(GameObject element, BoardPosition position) {
            boardMatrix[position.x, position.y, position.z, position.w] = element;
            if(IsElementWhite(element)) {
                whitePieces.Add(element, new List<Move>());
            } else if(IsElementBlack(element)) {
                blackPieces.Add(element, new List<Move>());
            }
        }

        public BoardPosition TransformIntoBoardPosition(Vector3 position) { // TODO Must change this to handle w values
            return new BoardPosition((int)position.x, (int)position.y, (int)position.z, 0);
        }

        public void StartTurn() {
            // Calculate possible moves for each piece of the playing team
            ref Dictionary<GameObject, List<Move>> playerPieces = ref (isWhiteTurn ? ref whitePieces : ref blackPieces);
            List<GameObject> keys = new List<GameObject>(playerPieces.Keys);
            foreach(GameObject piece in keys) {
                IMoveable[] moveables = piece.GetComponents<IMoveable>();
                List<Move> pieceMoves = new List<Move>();
                foreach(IMoveable moveable in moveables) {
                    moveable.GenerateMovesToList(ref pieceMoves);
                }
                playerPieces[piece] = pieceMoves;
            }
        }

        public void EndTurn() {
            isWhiteTurn = !isWhiteTurn;
        }
        private ClickablePiece selected_piece = null;
        public void SelectPiece(ClickablePiece piece) {
            if(selected_piece != null) {
                selected_piece.Deselect();
            }
            selected_piece = piece;
            List<Move> possibleMoves = (isWhiteTurn ? whitePieces[piece.gameObject] : blackPieces[piece.gameObject]);
            foreach(Move move in possibleMoves) {
                Debug.Log(move.end_position.x + " " + move.end_position.y + " " + move.end_position.z + " " + move.end_position.w);
                // TODO: Show possible moves on the screen
            }
        }

        // Possible Moves
        public bool CheckMove(Move move) {
            return true;
        }

        private bool IsElementBlock(GameObject element) {
            return element is not null && element.tag[0] == '#';
        }

        private bool IsElementWhite(GameObject element) {
            return element is not null && Char.IsUpper(element.tag[0]);
        }

        private bool IsElementBlack(GameObject element) {
            return element is not null && Char.IsLower(element.tag[0]);
        }
    }
}