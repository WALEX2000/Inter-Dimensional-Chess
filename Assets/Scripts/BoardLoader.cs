namespace Chess.Game
{
    using System.Collections;
    using System.Collections.Generic;
    using System;
    using UnityEngine;
    using Chess.Board;

    public class BoardLoader : MonoBehaviour
    {
        public BoardScriptableObject board_scriptable_object;

        public Transform board_parent;

        private void Start()
        { // TODO: Remove this later
          // load the board
            LoadBoard();
            GameManager.Instance.GameBoard.StartTurn();
        }
        void LoadBoard()
        {
            foreach (BoardElement board_element in board_scriptable_object.board_elem_list)
            {
                char piece_val = board_element.element_value;
                BoardPosition position = board_element.position;

                GameObject element = null;
                switch (piece_val)
                {
                    case '#': // Block
                        element = InstantiateBlock("Block", position);
                        break;
                    case 'P': // White pawn
                    case 'p': // Black pawn
                        element = InstantiatePiece("Pawn", piece_val, position);
                        break;
                    case 'R': // White rook
                    case 'r': // Black rook
                        element = InstantiatePiece("Rook", piece_val, position);
                        break;
                    case 'N': // White knight
                    case 'n': // Black knight
                        element = InstantiatePiece("Knight", piece_val, position);
                        break;
                    case 'B': // White bishop
                    case 'b': // Black bishop
                        element = InstantiatePiece("Bishop", piece_val, position);
                        break;
                    case 'Q': // White queen
                    case 'q': // Black queen
                        element = InstantiatePiece("Queen", piece_val, position);
                        break;
                    case 'K': // White king 
                    case 'k': // Black king
                        element = InstantiatePiece("King", piece_val, position);
                        break;
                    default: // empty
                        element = null;
                        break;
                }

                GameManager.Instance.GameBoard.AddBoardElement(element, position);
            }
        }

        private GameObject InstantiateBlock(string block_name, BoardPosition position)
        {
            GameObject block = (GameObject)Instantiate(Resources.Load("Prefabs/Blocks/" + block_name), new Vector3(position.x, position.y, position.z), Quaternion.identity);
            block.transform.parent = board_parent;
            int index_sum = position.x + position.y + position.z;
            if (index_sum % 2 == 0)
            {
                block.GetComponent<Renderer>().material = board_scriptable_object.color_theme_list[position.w].light_block_mat;
            }
            else
            {
                block.GetComponent<Renderer>().material = board_scriptable_object.color_theme_list[position.w].dark_block_mat;
            }

            return block;
        }

        private GameObject InstantiatePiece(string piece_name, char piece_val, BoardPosition position)
        {
            float y_pos = position.y - 0.5f;
            GameObject piece = (GameObject)Instantiate(Resources.Load("Prefabs/Pieces/" + piece_name), new Vector3(position.x, y_pos, position.z), Quaternion.identity);
            piece.transform.parent = board_parent;
            piece.tag = piece_val.ToString();
            if (Char.IsLower(piece_val))
            { // Black piece
                piece.GetComponent<Renderer>().material = board_scriptable_object.color_theme_list[position.w].dark_piece_mat;
                piece.GetComponent<Transform>().Rotate(0, 180, 0); // Make sure the piece is facing the right way
            }
            else
            { // White piece
                piece.GetComponent<Renderer>().material = board_scriptable_object.color_theme_list[position.w].light_piece_mat;
            }

            return piece;
        }
    }

}