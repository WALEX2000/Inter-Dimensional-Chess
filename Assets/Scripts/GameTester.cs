using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class GameTester : MonoBehaviour {
    public Transform board;
    private char?[,,] boardMatrix = new char?[10, 10, 10];
    // Start is called before the first frame update
    private void Start() {
        // get children pieces of board
        Transform[] boardPieces = board.GetComponentsInChildren<Transform>();
        boardPieces = boardPieces[1..];

        // round position of all board pieces
        foreach (Transform piece in boardPieces)
        {
            if(piece.tag == "#") {
                piece.position = new Vector3(Mathf.Round(piece.position.x), Mathf.Round(piece.position.y), Mathf.Round(piece.position.z));
                boardMatrix[(int)piece.localPosition.x, (int)piece.localPosition.y, (int)piece.localPosition.z] = piece.tag[0];
            } else if (piece.tag != "Untagged") {
                int yPos = (int) Mathf.Ceil(piece.position.y);
                piece.position = new Vector3(Mathf.Round(piece.position.x), yPos - 0.5f, Mathf.Round(piece.position.z));
                boardMatrix[(int)piece.localPosition.x, yPos, (int)piece.localPosition.z] = piece.tag[0];
            }
        }
        Debug.Log("Started!");

        // Create BoardSO
        Chess.Board.BoardScriptableObject boardSO = ScriptableObject.CreateInstance<Chess.Board.BoardScriptableObject>();
        // set the first x y and z elements of boardSO.boardMatrix to boardMatrix
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                for (int z = 0; z < 10; z++)
                {
                    if(boardMatrix[x, y, z] != null) {
                        // create new board element
                        boardSO.board_elem_list.Add(new Chess.Board.BoardElement(boardMatrix[x, y, z].Value, x, y, z, 0));
                    }
                }
            }
        }

        boardSO.board_name = "Test Board";
        // save the boardSO
        AssetDatabase.CreateAsset(boardSO, "Assets/Resources/Boards/BoardSO.asset");
        AssetDatabase.SaveAssets();
    }

    public void ClickedOnPiece(int x, int y, int z)
    {
        Debug.Log("Clicked on piece at " + x + ", " + y + ", " + z);
        Debug.Log("Found: " + boardMatrix[x, y, z]);
    }
}