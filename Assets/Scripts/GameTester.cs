using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Chess.Board;

public class GameTester : MonoBehaviour {
    public Transform board;
    public string board_name;
    private char?[,,] boardMatrix = new char?[10, 10, 10];
    private List<BoardElement> board_element_list = new List<BoardElement>();
    // Start is called before the first frame update
    private void Start() {
        // get children pieces of board
        Transform[] boardPieces = board.GetComponentsInChildren<Transform>();
        boardPieces = boardPieces[1..];

        // round position of all board pieces and add to list
        foreach (Transform piece in boardPieces)
        {
            if(piece.tag == "#") {
                piece.localPosition = new Vector3(Mathf.Round(piece.localPosition.x), Mathf.Round(piece.localPosition.y), Mathf.Round(piece.localPosition.z));
                board_element_list.Add(new BoardElement(piece.tag[0], (int)piece.localPosition.x, (int)piece.localPosition.y, (int)piece.localPosition.z, 0));
            } else if (piece.tag != "Untagged") {
                int yPos = (int) Mathf.Ceil(piece.localPosition.y);
                piece.localPosition = new Vector3(Mathf.Round(piece.localPosition.x), yPos - 0.5f, Mathf.Round(piece.localPosition.z));
                board_element_list.Add(new BoardElement(piece.tag[0], (int)piece.localPosition.x, yPos, (int)piece.localPosition.z, 0));
            } else { // skip unknwon piece
                continue;
            }
        }

        // Create BoardSO
        Chess.Board.BoardScriptableObject boardSO = ScriptableObject.CreateInstance<Chess.Board.BoardScriptableObject>();

        boardSO.board_elem_list = board_element_list;
        boardSO.board_name = board_name;
        // save the boardSO
        AssetDatabase.CreateAsset(boardSO, "Assets/Resources/Boards/"+board_name+".asset");
        AssetDatabase.SaveAssets();
    }

    public void ClickedOnPiece(int x, int y, int z)
    {
        Debug.Log("Clicked on piece at " + x + ", " + y + ", " + z);
        Debug.Log("Found: " + boardMatrix[x, y, z]);
    }
}