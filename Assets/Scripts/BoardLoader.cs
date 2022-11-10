namespace Chess.Game
{
    using System.Collections;
    using System.Collections.Generic;
    using System;
    using UnityEngine;
    using Chess.Board;

    public class BoardLoader : MonoBehaviour
    {
        public BoardSO boardScriptableObject;
        private const int X_DIM_OFFSET = 10;
        private const int Y_DIM_OFFSET = 0;
        private const int Z_DIM_OFFSET = 0;

        private void Start()
        {
            GameManager.Instance.CreateBoard(boardScriptableObject.GetMaxBoardSize());
            LoadDimensions();
            GameManager.Instance.StartTurn();
        }

        private void LoadDimensions() {
            for (int i = 0; i < boardScriptableObject.dimensions.Count; i++)
            {
                // Create empty child of boardParent
                GameObject dimensionParent = new GameObject(i.ToString());
                dimensionParent.transform.parent = GameManager.Instance.boardTransform;
                dimensionParent.transform.localPosition = new Vector3(i*X_DIM_OFFSET,i*Y_DIM_OFFSET,i*Z_DIM_OFFSET);
                dimensionParent.AddComponent<DimensionObject>().colorTheme = boardScriptableObject.dimensions[i].colorTheme;
                GameObject dimensionGUIHolder = new GameObject(GameManager.GUIHolderObjectName);
                dimensionGUIHolder.transform.parent = dimensionParent.transform;
                dimensionGUIHolder.transform.localPosition = Vector3.zero;

                GameManager.Instance.gameBoard.AddDimensionObject(dimensionParent);
                GameManager.Instance.gameBoard.setPromotionRank(i, boardScriptableObject.dimensions[i].maxDimensionRank, white: true);
                GameManager.Instance.gameBoard.setPromotionRank(i, boardScriptableObject.dimensions[i].minDimensionRank, white: false);
                InstantiateDimensionElements(boardScriptableObject.dimensions[i], dimensionParent.transform);
            }
        }

        void InstantiateDimensionElements(Dimension dimension, Transform parentTransform)
        {
            foreach (BoardElement board_element in dimension.dimensionElements)
            {
                char piece_val = board_element.element_value;
                BoardPosition position = board_element.position;

                GameObject element = null;
                switch (piece_val)
                {
                    case '#': // Block
                        element = InstantiateBlock("Block", position, dimension.colorTheme, parentTransform);
                        break;
                    case 'P': // White pawn
                    case 'p': // Black pawn
                        element = InstantiatePiece("Pawn", piece_val, position, dimension.colorTheme, parentTransform);
                        break;
                    case 'R': // White rook
                    case 'r': // Black rook
                        element = InstantiatePiece("Rook", piece_val, position, dimension.colorTheme, parentTransform);
                        break;
                    case 'N': // White knight
                    case 'n': // Black knight
                        element = InstantiatePiece("Knight", piece_val, position, dimension.colorTheme, parentTransform);
                        break;
                    case 'B': // White bishop
                    case 'b': // Black bishop
                        element = InstantiatePiece("Bishop", piece_val, position, dimension.colorTheme, parentTransform);
                        break;
                    case 'Q': // White queen
                    case 'q': // Black queen
                        element = InstantiatePiece("Queen", piece_val, position, dimension.colorTheme, parentTransform);
                        break;
                    case 'K': // White king 
                    case 'k': // Black king
                        element = InstantiatePiece("King", piece_val, position, dimension.colorTheme, parentTransform);
                        break;
                    default: // empty
                        element = null;
                        break;
                }
            }
            int w = dimension.dimensionElements[0].position.w;
            InstatiateTower(dimension.colorTheme, parentTransform, w);
        }

        private GameObject InstantiateBlock(string block_name, BoardPosition position, ColorThemeSO colorTheme, Transform parentTransform, bool addToBoard = true)
        {
            GameObject block = (GameObject)Instantiate(Resources.Load("Prefabs/Blocks/" + block_name));
            block.transform.parent = parentTransform;
            block.transform.localPosition = new Vector3(position.x, position.y, position.z);
            int index_sum = position.x + position.y + position.z;
            if (index_sum % 2 == 0)
            {
                block.GetComponent<Renderer>().material = colorTheme.light_block_mat;
            }
            else
            {
                block.GetComponent<Renderer>().material = colorTheme.dark_block_mat;
            }

            if(addToBoard) GameManager.Instance.gameBoard.AddBoardElement(block, position);
            else GameManager.Instance.gameBoard.AddTowerBlock(block, position);
            return block;
        }

        private void InstatiateTower(ColorThemeSO colorTheme, Transform parentTransform, int dimension) {
            for(int y = 0; y <= 21; y++) {
                for(int x = 0; x < 6; x++) {
                    for(int z = 0; z < 6; z++) {
                        InstantiateBlock("Block", new BoardPosition(x, y, z, dimension), colorTheme, parentTransform, addToBoard: false);
                    }
                }
            }
        }

        public static GameObject InstantiatePiece(string piece_name, char piece_val, BoardPosition position, ColorThemeSO colorTheme, Transform parentTransform)
        {
            GameObject piece = (GameObject)Instantiate(Resources.Load("Prefabs/Pieces/" + piece_name));
            piece.transform.parent = parentTransform;
            float y_pos = position.y - 0.5f;
            piece.transform.localPosition = new Vector3(position.x, y_pos, position.z);

            piece.tag = piece_val.ToString();
            if (Char.IsLower(piece_val))
            { // Black piece
                piece.GetComponent<Renderer>().material = colorTheme.dark_piece_mat;
                piece.GetComponent<Transform>().Rotate(0, 180, 0); // This Makes sure the piece is facing the right way
            }
            else
            { // White piece
                piece.GetComponent<Renderer>().material = colorTheme.light_piece_mat;
            }

            GameManager.Instance.gameBoard.AddBoardElement(piece, position);
            return piece;
        }
    }

}