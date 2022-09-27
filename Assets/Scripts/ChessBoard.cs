namespace Chess.Board {
    
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Chess.Pieces;
    using Chess.Game;
    using System;

    public class ChessBoard {
        public readonly int MAX_X_SIZE = 12;
        public readonly int MAX_Y_SIZE = 12;
        public readonly int MAX_Z_SIZE = 12;
        public readonly int MAX_W_SIZE = 6;
        private GameObject[,,,] boardMatrix;
        private List<Move> moveList = new List<Move>();
        private Dictionary<GameObject, List<Move>> whitePieces = new Dictionary<GameObject, List<Move>>();
        private Dictionary<GameObject, List<Move>> blackPieces = new Dictionary<GameObject, List<Move>>();
        private ClickablePiece selected_piece = null;

        public ChessBoard(int xSize = 12, int ySize = 12, int zSize = 12, int wSize = 6) {
            MAX_X_SIZE = xSize;
            MAX_Y_SIZE = ySize;
            MAX_Z_SIZE = zSize;
            MAX_W_SIZE = wSize;
            boardMatrix = new GameObject[MAX_X_SIZE, MAX_Y_SIZE, MAX_Z_SIZE, MAX_W_SIZE];
        }

        public GameObject GetBoardElement(BoardPosition pos) {
            try {
                return boardMatrix[pos.x, pos.y, pos.z, pos.w];
            } catch (IndexOutOfRangeException) {
                return null; // TODO I need to handle this better somehow
            }
        }

        public void AddBoardElement(GameObject element, BoardPosition position) {
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

        public Vector3 BoardPositionIntoVector3(BoardPosition position) { // TODO Must change this to handle w values
            float yPos = position.y - 0.5f;
            return new Vector3(position.x, yPos, position.z);
        }

        public void SelectPiece(ClickablePiece piece) {
            if(selected_piece != null) {
                selected_piece.Deselect();
                DestroySelectMarkers();
            }
            selected_piece = piece;
            List<Move> possibleMoves = (GameManager.Instance.isWhiteTurn ? whitePieces[piece.gameObject] : blackPieces[piece.gameObject]);
            foreach(Move move in possibleMoves) {
                // TODO: Show possible moves on the screen (properly)
                GameObject moveIndicator = GameObject.Instantiate(GameManager.Instance.moveIndicatorPrefab, GameManager.Instance.tmp);
                moveIndicator.transform.localPosition = new Vector3(move.end_position.x, move.end_position.y, move.end_position.z);
                moveIndicator.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            }
        }

        // Game Functions
        public void StartTurn() {
            // Calculate possible moves for each piece of the playing team
            ref Dictionary<GameObject, List<Move>> playerPieces = ref (GameManager.Instance.isWhiteTurn ? ref whitePieces : ref blackPieces);
            List<GameObject> keys = new List<GameObject>(playerPieces.Keys);
            foreach(GameObject piece in keys) {
                PieceMovement[] moveables = piece.GetComponents<PieceMovement>();
                List<Move> pieceMoves = new List<Move>();
                foreach(PieceMovement moveable in moveables) {
                    moveable.GenerateMovesToList(ref pieceMoves);
                }
                playerPieces[piece] = pieceMoves;
            }
        }

        public void EndTurn() {
            GameManager.Instance.isWhiteTurn = !GameManager.Instance.isWhiteTurn;
            StartTurn();
        }

        private void DestroySelectMarkers() {
            while (GameManager.Instance.tmp.childCount > 0) { // TODO Change this
                GameObject.DestroyImmediate(GameManager.Instance.tmp.GetChild(0).gameObject);
            }
        }

        public void MovePiece(Transform moveIndicator) {
            BoardPosition movePosition = TransformIntoBoardPosition(moveIndicator);
            BoardPosition originalPosition = TransformIntoBoardPosition(selected_piece.transform);
            selected_piece.Deselect();
            DestroySelectMarkers();
            // TODO: This isn't very good practice but it works for now
            PieceMovement pieceMovement = selected_piece.GetComponent<PieceMovement>();
            pieceMovement.MovePiece();
            // Delete the eaten piece, if any
            GameObject enemyPiece = GetBoardElement(movePosition);
            if(enemyPiece != null) {
                if(IsElementWhite(enemyPiece)) {
                    whitePieces.Remove(enemyPiece);
                } else if(IsElementBlack(enemyPiece)) {
                    blackPieces.Remove(enemyPiece);
                }
                GameObject.Destroy(enemyPiece);
            }
            // Change the position of the selected piece on the Board Matrix
            boardMatrix[movePosition.x, movePosition.y, movePosition.z, movePosition.w] = selected_piece.gameObject;
            boardMatrix[originalPosition.x, originalPosition.y, originalPosition.z, originalPosition.w] = null;
            // Change the position of the selected piece on the Game world
            Vector3 piecePosition = BoardPositionIntoVector3(movePosition);
            selected_piece.transform.localPosition = piecePosition;
            selected_piece = null;
            EndTurn();
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
            // Can't fly TODO: this doesn't work for "w" i think
            BoardPosition below_end_pos = new BoardPosition(move.end_position.x, move.end_position.y - 1, move.end_position.z, move.end_position.w);
            if(!IsElementBlock(GetBoardElement(below_end_pos))) { // This element has to be a block
                // Debug.Log("Position at (" + move.end_position.x + "," + move.end_position.y + "," + move.end_position.z + "," + move.end_position.w + ") is floating!");
                return MoveOutcome.Invalid; // TODO This is dependent upon GetBoardElement returning null if the position is out of bounds
            }
            // Same team piece
            if (IsTeamEqual(piece, endObj)) {
                // Debug.Log("Object at (" + move.end_position.x + "," + move.end_position.y + "," + move.end_position.z + "," + move.end_position.w + ") is a piece of the same team!");
                return MoveOutcome.FriendlyCapture;
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