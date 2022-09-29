namespace Chess.Board {
    
    using System.Collections.Generic;
    using UnityEngine;
    using Chess.Pieces;
    using Chess.Game;
    using System;

    public class ChessBoard
    { /* Holds all the infromation about the the board:
        - Board Size
        - Board structure across all dimensions
        - Piece position
        - Possible Piece movement
        - Piece movement history
        */
        public int MAX_X_SIZE {get; private set;}
        public int MAX_Y_SIZE {get; private set;}
        public int MAX_Z_SIZE {get; private set;}
        public int MAX_W_SIZE {get; private set;}
        private GameObject[,,,] boardMatrix;
        private List<Move> moveHistory = new List<Move>();
        private Dictionary<GameObject, List<Move>> whitePiecesMoveCollection = new Dictionary<GameObject, List<Move>>();
        private Dictionary<GameObject, List<Move>> blackPiecesMoveCollection = new Dictionary<GameObject, List<Move>>();
        private List<GameObject> dimensionObjects = new List<GameObject>(); // TODO: Maybe move this somewhere else so this class is just about the board

        // Board General Methods
        public ChessBoard(int xSize = 12, int ySize = 12, int zSize = 12, int wSize = 6) {
            MAX_X_SIZE = xSize;
            MAX_Y_SIZE = ySize;
            MAX_Z_SIZE = zSize;
            MAX_W_SIZE = wSize;
            boardMatrix = new GameObject[MAX_X_SIZE, MAX_Y_SIZE, MAX_Z_SIZE, MAX_W_SIZE];
        }

        public void AddBoardElement(GameObject element, BoardPosition position) {
            boardMatrix[position.x, position.y, position.z, position.w] = element;
            if(IsElementWhite(element)) {
                whitePiecesMoveCollection.Add(element, new List<Move>());
            } else if(IsElementBlack(element)) {
                blackPiecesMoveCollection.Add(element, new List<Move>());
            }
        }

        public GameObject GetBoardElement(BoardPosition pos) {
            try {
                return boardMatrix[pos.x, pos.y, pos.z, pos.w];
            } catch (IndexOutOfRangeException) {
                return null; // TODO I need to handle this better somehow
            }
        }

        public void AddDimensionObject(GameObject dimensionObject) {
            dimensionObjects.Add(dimensionObject);
        }

        public BoardPosition TransformIntoBoardPosition(Transform transform) {
            Vector3 position = transform.localPosition;
            int yPos = (int) Mathf.Ceil(position.y);
            int wPos = int.Parse(transform.parent.name);
            return new BoardPosition((int)position.x, yPos, (int)position.z, wPos);
        }

        private GameObject GetDimensionObject(BoardPosition position) { // TODO: Handle IndexOutOfRange.
            return dimensionObjects[position.w];
        }

        private Vector3 BoardPositionIntoVector3(BoardPosition position) {
            float yPos = position.y - 0.5f;
            return new Vector3(position.x, yPos, position.z);
        }

        // Piece Movement / Selection Methods
        public void MakeMove(Move move) {
            BoardPosition startPosition = move.startPosition;
            BoardPosition endPosition = move.endPosition;
            GameObject movedPiece = GetBoardElement(startPosition);
            
            // TODO: This isn't very good practice but it works for now
            PieceMovement pieceMovement = movedPiece.GetComponent<PieceMovement>();
            pieceMovement.MovePiece();
            // Delete the eaten piece, if any
            GameObject enemyPiece = GetBoardElement(endPosition);
            if(enemyPiece != null) {
                if(IsElementWhite(enemyPiece)) {
                    whitePiecesMoveCollection.Remove(enemyPiece);
                } else if(IsElementBlack(enemyPiece)) {
                    blackPiecesMoveCollection.Remove(enemyPiece);
                }
                GameObject.Destroy(enemyPiece);
            }
            // Change the position of the selected piece on the Board Matrix
            boardMatrix[endPosition.x, endPosition.y, endPosition.z, endPosition.w] = movedPiece;
            boardMatrix[startPosition.x, startPosition.y, startPosition.z, startPosition.w] = null;
            
            // Change the position of the selected piece on the Game world
            movedPiece.transform.parent = GetDimensionObject(endPosition).transform;
            Vector3 piecePosition = BoardPositionIntoVector3(endPosition);
            movedPiece.transform.localPosition = piecePosition;
            moveHistory.Add(move);

            GameManager.Instance.DeselectPiece();
            GameManager.Instance.EndTurn();
        }

        // Possible Moves
        public MoveOutcome CheckMoveRules(GameObject piece, Move move) { // Checks if Move is valid according to Board Rules
            // TODO: Remove Debugs
            // CHECK ALL GAME RULES
            // Can't move into blocks
            GameObject endObj = GetBoardElement(move.endPosition);
            if (IsElementBlock(endObj)) {
                // Debug.Log("Object at (" + move.end_position.x + "," + move.end_position.y + "," + move.end_position.z + "," + move.end_position.w + ") is a block!");
                return MoveOutcome.Invalid;
            }
            // Can't fly TODO: this doesn't work for "w" i think
            BoardPosition below_end_pos = new BoardPosition(move.endPosition.x, move.endPosition.y - 1, move.endPosition.z, move.endPosition.w);
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

        public List<Move> GetPieceMoves(GameObject piece) {
            if(IsElementWhite(piece)) {
                return whitePiecesMoveCollection[piece];
            } else if(IsElementBlack(piece)) {
                return blackPiecesMoveCollection[piece];
            } else return null;
        }

        public void CalculateTeamMoves(bool whiteTeam) {
            // Calculate possible moves for each piece of the playing team
            ref Dictionary<GameObject, List<Move>> playerPieces = ref (whiteTeam ? ref whitePiecesMoveCollection : ref blackPiecesMoveCollection);
            List<GameObject> keys = new List<GameObject>(playerPieces.Keys);
            foreach(GameObject piece in keys) {
                PieceMovement[] moveables = piece.GetComponents<PieceMovement>();
                List<Move> pieceMoves = new List<Move>();
                foreach(PieceMovement moveable in moveables) {
                    moveable.GenerateMovesToList(ref pieceMoves);
                }
                foreach(Move move in pieceMoves) {
                    // Check if there are any Checks or Checkmates
                }
                playerPieces[piece] = pieceMoves;
            }
        }

        // Misc Methods
        public bool isKing(GameObject piece) {
            return piece.tag == "k" || piece.tag == "K";
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