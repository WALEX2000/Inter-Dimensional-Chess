using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Chess.Board;

public class MapLoader : MonoBehaviour
{
    public BoardScriptableObject board_scriptable_object;

    public Transform board_parent;
    void Start()
    {
        foreach (BoardElement board_element in board_scriptable_object.board_elem_list)
        {
            char piece_val = board_element.element_value;
            int x = board_element.x;
            int y = board_element.y;
            int z = board_element.z;
            int w = board_element.w;

            switch(piece_val) {
                case 'P': // White pawn
                case 'p': // Black pawn
                    InstantiatePiece("Pawn", piece_val, x, y, z, w);
                    break;
                case 'R': // White rook
                case 'r': // Black rook
                    InstantiatePiece("Rook", piece_val, x, y, z, w);
                    break;
                case 'N': // White knight
                case 'n': // Black knight
                    InstantiatePiece("Knight", piece_val, x, y, z, w);
                    break;
                case 'B': // White bishop
                case 'b': // Black bishop
                    InstantiatePiece("Bishop", piece_val, x, y, z, w);
                    break;
                case 'Q': // White queen
                case 'q': // Black queen
                    InstantiatePiece("Queen", piece_val, x, y, z, w);
                    break;
                case 'K': // White king 
                case 'k': // Black king
                    InstantiatePiece("King", piece_val, x, y, z, w);
                    break;   
                case '#': // Block
                    InstantiateBlock(x, y, z, w, "Block");
                    break;
                default: // empty
                    break;
            }
        }
    }

    private void InstantiateBlock(int x, int y, int z, int w, string block_name) {
        GameObject block = (GameObject)Instantiate(Resources.Load("Prefabs/Blocks/"+block_name), new Vector3(x, y, z), Quaternion.identity);
        block.transform.parent = board_parent;
        int index_sum = x + y + z;
        if (index_sum % 2 == 0)
        {
            block.GetComponent<Renderer>().material = board_scriptable_object.color_theme_list[w].light_block_mat;
        }
        else
        {
            block.GetComponent<Renderer>().material = board_scriptable_object.color_theme_list[w].dark_block_mat;
        }
    }

    private void InstantiatePiece( string piece_name, char piece_val, int x, int y, int z, int w) {
        float y_pos = y - 0.5f;
        GameObject piece = (GameObject)Instantiate(Resources.Load("Prefabs/Pieces/"+piece_name), new Vector3(x, y_pos, z), Quaternion.identity);
        piece.transform.parent = board_parent;
        piece.tag = piece_val.ToString();
        if(Char.IsLower(piece_val)) {
            piece.GetComponent<Renderer>().material = board_scriptable_object.color_theme_list[w].dark_piece_mat;
        } else {
            piece.GetComponent<Renderer>().material = board_scriptable_object.color_theme_list[w].light_piece_mat;
        }
    }
}
