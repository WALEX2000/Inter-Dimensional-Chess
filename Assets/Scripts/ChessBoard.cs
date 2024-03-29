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
        public readonly int[] boardBoundaries;
        private int[] whitePromotionRanks;
        private int[] blackPromotionRanks;
        private GameObject[,,,] boardMatrix;
        private List<Move> moveHistory = new List<Move>();
        private Dictionary<GameObject, List<Move>> whitePiecesMoveCollection = new Dictionary<GameObject, List<Move>>();
        private Dictionary<GameObject, List<Move>> blackPiecesMoveCollection = new Dictionary<GameObject, List<Move>>();
        private List<AfterImage> enPassantGhosts = new List<AfterImage>();
        private List<GameObject> dimensionObjects = new List<GameObject>(); // TODO: Maybe move this somewhere else so this class is just about the board
        private GameObject[,,,] towerMatrix;

        // Board General Methods
        public ChessBoard(int xSize = 12, int ySize = 12, int zSize = 12, int wSize = 6) {
            boardBoundaries = new int[] { xSize, ySize, zSize, wSize };
            boardMatrix = new GameObject[xSize, ySize, zSize, wSize];
            towerMatrix = new GameObject[7, 22, 7, wSize];
            whitePromotionRanks = new int[wSize];
            blackPromotionRanks = new int[wSize];
        }

        public GameObject[,,,] getBoardMatrix() {
            return boardMatrix;
        }

        public GameObject[,,,] getTowerMatrix() {
            return towerMatrix;
        }

        public BoardPosition GetPieceBoardPosition(GameObject piece) {
            for (int w = 0; w < boardBoundaries[3]; w++) {
                for (int x = 0; x < boardBoundaries[0]; x++) {
                    for (int y = 0; y < boardBoundaries[1]; y++) {
                        for (int z = 0; z < boardBoundaries[2]; z++) {
                            if (boardMatrix[x, y, z, w] == piece) {
                                return new BoardPosition(x, y, z, w);
                            }
                        }
                    }
                }
            }
            throw new Exception("Piece not found on board");
        }

        public void setPromotionRank(int dimension, int rank, bool white) {
            int[] promotionRanks = white ? whitePromotionRanks : blackPromotionRanks;
            promotionRanks[dimension] = rank;
        }

        public void AddBoardElement(GameObject element, BoardPosition position) {
            boardMatrix[position.x, position.y, position.z, position.w] = element;
            if(IsElementWhite(element)) {
                whitePiecesMoveCollection.Add(element, new List<Move>());
            } else if(IsElementBlack(element)) {
                blackPiecesMoveCollection.Add(element, new List<Move>());
            }
        }
        public void AddTowerBlock(GameObject block, BoardPosition position) {
            towerMatrix[position.x, position.y, position.z, position.w] = block;
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

        // public BoardPosition TransformIntoBoardPosition(Transform transform) {
        //     Vector3 position = transform.localPosition;
        //     int yPos = (int) Mathf.Ceil(position.y);
        //     int wPos = int.Parse(transform.parent.name);
        //     return new BoardPosition((int)position.x, yPos, (int)position.z, wPos);
        // }

        private GameObject GetDimensionObject(BoardPosition position) { // TODO: Handle IndexOutOfRange.
            return dimensionObjects[position.w];
        }

        private Vector3 BoardPositionIntoVector3(BoardPosition position) {
            float yPos = position.y - 0.5f;
            return new Vector3(position.x, yPos, position.z);
        }

        // Piece Movement
        public void MakeMove(Move move) {
            BoardPosition startPosition = move.startPosition;
            BoardPosition endPosition = move.endPosition;
            GameObject movedPiece = GetBoardElement(startPosition);
            
            // TODO: This isn't very good practice but it works for now
            PieceMovement pieceMovement = movedPiece.GetComponent<PieceMovement>();
            pieceMovement.MovePiece();

            // Delete the eaten piece, if any
            GameObject endObj = GetBoardElement(endPosition);
            if((move.outcome & MoveOutcome.Capture) != 0)
            { // This is a capture move
                GameObject objToDestroy = endObj;
                if ((move.outcome & MoveOutcome.EnPassant) != 0) {
                    Move lastMove = moveHistory[moveHistory.Count - 1];
                    objToDestroy = lastMove.afterImage.afterImageOwner;
                    BoardPosition destroyPosition = GetPieceBoardPosition(objToDestroy);
                    boardMatrix[destroyPosition.x, destroyPosition.y, destroyPosition.z, destroyPosition.w] = null;
                }

                if(IsElementWhite(objToDestroy)) {
                    whitePiecesMoveCollection.Remove(objToDestroy);
                } else if(IsElementBlack(objToDestroy)) {
                    blackPiecesMoveCollection.Remove(objToDestroy);
                }
                GameObject.Destroy(objToDestroy);
            }

            // Change the position of the selected piece on the Board Matrix
            boardMatrix[endPosition.x, endPosition.y, endPosition.z, endPosition.w] = movedPiece;
            boardMatrix[startPosition.x, startPosition.y, startPosition.z, startPosition.w] = null;
            
            // Change the position of the selected piece on the Game world
            movedPiece.transform.parent = GetDimensionObject(endPosition).transform;
            Vector3 piecePosition = BoardPositionIntoVector3(endPosition);
            movedPiece.transform.localPosition = piecePosition;

            // Handle Promotions
            if(move.promotionType != null) {
                // TODO: Add menu to choose promotion type
                move.promotionType = 'Q';
                HandlePromotion(movedPiece, endPosition, (char) move.promotionType);
            }
            
            moveHistory.Add(move);
            GameManager.Instance.boardInterface.DeselectPiece();
            GameManager.Instance.EndTurn();
        }

        public void CheckMoveOutcome(GameObject piece, ref Move move) { // Checks if Move is valid according to Board Rules
            GameObject endObj = GetBoardElement(move.endPosition);
            if (IsElementBlock(endObj))
            { // If the end position is a block, the move is invalid
                move.outcome = MoveOutcome.Invalid;
                return;
            }
            
            BoardPosition below_end_pos = new BoardPosition(move.endPosition.x, move.endPosition.y - 1, move.endPosition.z, move.endPosition.w);
            if(!IsElementBlock(GetBoardElement(below_end_pos)))
            { // If the element below board position is not a block, the move is invalid
                move.outcome = MoveOutcome.Invalid;
                return;
                // TODO This is dependent upon GetBoardElement returning null if the position is out of bounds
            }
            
            if (endObj is null)
            { // If the end position is empty, the move is valid
                move.outcome |= MoveOutcome.BasicMove;

                if(moveHistory.Count > 0) {
                    Move lastMove = moveHistory[moveHistory.Count - 1];
                    if ((lastMove.outcome & MoveOutcome.AfterImage) != 0 &&
                        lastMove.afterImage.afterImagePosition.IsEqual(move.endPosition) &&
                        !IsTeamEqual(piece, lastMove.afterImage.afterImageOwner))
                    { // Passing into an enemy after image
                        move.outcome |= MoveOutcome.EnPassant;
                    }
                }
            }
            else if (IsTeamEqual(piece, endObj))
            { // If the end position is a piece of the same team, the move is a FriendlyCapture
                move.outcome |= MoveOutcome.FriendlyCapture;
            }
            else if(endObj != null && !IsTeamEqual(piece, endObj))
            { // If the end position is a piece of the opposite team, the move is a Capture
                move.outcome |= MoveOutcome.Capture;
            }

            if(IsElementWhite(piece) && move.endPosition.z == whitePromotionRanks[move.endPosition.w]) {
                move.outcome |= MoveOutcome.PromotionRank;
            } else if(IsElementBlack(piece) && move.endPosition.z == blackPromotionRanks[move.endPosition.w]) {
                move.outcome |= MoveOutcome.PromotionRank;
            }

            // TODO: Check if the move is a Castling
            // TODO: Check if the move is putting my king in check
            // TODO: Check if the move is putting the opoosing king in check
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
                playerPieces[piece] = pieceMoves;
            }
        }

        private void HandlePromotion(GameObject movedPiece, BoardPosition position, char promotionType) {
            
            if(Char.IsLower(movedPiece.tag[0])) promotionType = Char.ToLower(promotionType);
            else promotionType = Char.ToUpper(promotionType);
            Transform dimension = movedPiece.transform.parent;
            ColorThemeSO colorTheme = dimension.GetComponent<DimensionObject>().colorTheme;
            
            // remove from dictionary
            if(IsElementWhite(movedPiece)) {
                whitePiecesMoveCollection.Remove(movedPiece);
            } else if(IsElementBlack(movedPiece)) {
                blackPiecesMoveCollection.Remove(movedPiece);
            }
            boardMatrix[position.x, position.y, position.z, position.w] = null;
            GameObject.Destroy(movedPiece);

            // Create the new piece
            switch (promotionType)
            {
                case 'Q':
                case 'q':
                    BoardLoader.InstantiatePiece("Queen", promotionType, position, colorTheme, dimension);
                    break;
                case 'R':
                case 'r':
                    BoardLoader.InstantiatePiece("Rook", promotionType, position, colorTheme, dimension);
                    break;
                case 'B':
                case 'b':
                    BoardLoader.InstantiatePiece("Bishop", promotionType, position, colorTheme, dimension);
                    break;
                case 'N':
                case 'n':
                    BoardLoader.InstantiatePiece("Knight", promotionType, position, colorTheme, dimension);
                    break;
                default:
                    Debug.LogError("Invalid promotion type: " + promotionType);
                    break;
            }
        }

        // Misc Methods
        public bool IsKing(GameObject piece) {
            return piece.tag == "k" || piece.tag == "K";
        }

        public bool IsPawn(GameObject piece) {
            return piece.tag == "p" || piece.tag == "P";
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