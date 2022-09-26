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

        public Transform tmp; // TODO: Delete this

        // Game Functions
        private GameObject[,,,] boardMatrix = new GameObject[12, 12, 12, 12]; // Max Board Size (TODO Change to something more centralized)
        private Dictionary<GameObject, List<Move>> whitePieces = new Dictionary<GameObject, List<Move>>();
        private Dictionary<GameObject, List<Move>> blackPieces = new Dictionary<GameObject, List<Move>>();
        public bool isWhiteTurn = true;

        public GameObject GetBoardElement(BoardPosition pos) {
            try {
                return boardMatrix[pos.x, pos.y, pos.z, pos.w];
            } catch (IndexOutOfRangeException) {
                return null; // TODO I need to handle this better somehow
            }
        }
        public void SetBoardElement(GameObject element, BoardPosition position) {
            boardMatrix[position.x, position.y, position.z, position.w] = element;
            if(IsElementWhite(element)) {
                whitePieces.Add(element, new List<Move>());
            } else if(IsElementBlack(element)) {
                blackPieces.Add(element, new List<Move>());
            }
        }

        public BoardPosition TransformIntoBoardPosition(Transform transform) { // TODO Must change this to handle w values
            Vector3 position = transform.localPosition;
            int yPos = (int) Mathf.Ceil(position.y);
            return new BoardPosition((int)position.x, yPos, (int)position.z, 0);
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
            while (tmp.childCount > 0) {
                DestroyImmediate(tmp.GetChild(0).gameObject);
            }
            foreach(Move move in possibleMoves) {
                // TODO: Show possible moves on the screen (properly)
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.parent = tmp;
                sphere.transform.localPosition = new Vector3(move.end_position.x, move.end_position.y, move.end_position.z);
                sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
        }

        // Possible Moves
        public MoveOutcome CheckMoveRules(GameObject piece, Move move) { // Checks if Move is valid according to Board Rules
            // CHECK ALL GAME RULES
            // Can't move into blocks
            GameObject endObj = GetBoardElement(move.end_position);
            if (IsElementBlock(endObj)) {
                // Debug.Log("Object at (" + move.end_position.x + "," + move.end_position.y + "," + move.end_position.z + "," + move.end_position.w + ") is a block!");
                return MoveOutcome.Invalid;
            }
            // Can't move into same team
            if (IsTeamEqual(piece, endObj)) {
                // Debug.Log("Object at (" + move.end_position.x + "," + move.end_position.y + "," + move.end_position.z + "," + move.end_position.w + ") is a piece of the same team!");
                return MoveOutcome.Invalid;
            }
            // Can't fly TODO: this doesn't work for "w" i think
            BoardPosition below_end_pos = new BoardPosition(move.end_position.x, move.end_position.y - 1, move.end_position.z, move.end_position.w);
            if(!IsElementBlock(GetBoardElement(below_end_pos))) { // This element has to be a block
                // Debug.Log("Position at (" + move.end_position.x + "," + move.end_position.y + "," + move.end_position.z + "," + move.end_position.w + ") is floating!");
                return MoveOutcome.Invalid; // TODO This is dependent upon GetBoardElement returning null if the position is out of bounds
            }
            // Capture
            if(endObj != null && !IsTeamEqual(piece, endObj)) {
                // Debug.Log("Object at (" + move.end_position.x + "," + move.end_position.y + "," + move.end_position.z + "," + move.end_position.w + ") is a piece of the other team!");
                return MoveOutcome.Capture;
            }
            // CHECK ALL DIMENSION RULES
            // CHECK ALL PIECE SPECIFIC RULES
            // Debug.Log("VALID: (" + move.end_position.x + "," + move.end_position.y + "," + move.end_position.z + "," + move.end_position.w + ")");
            return MoveOutcome.Valid;
        }

        public bool IsElementBlock(GameObject element) {
            return element is not null && element.tag[0] == '#';
        }

        public bool IsElementWhite(GameObject element) {
            return element is not null && Char.IsUpper(element.tag[0]);
        }

        public bool IsElementBlack(GameObject element) {
            return element is not null && Char.IsLower(element.tag[0]);
        }

        public bool IsTeamEqual(GameObject element1, GameObject element2)
        {
            return (IsElementWhite(element1) && IsElementWhite(element2)) || (IsElementBlack(element1) && IsElementBlack(element2));
        }
    }
}