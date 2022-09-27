using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Chess.Board;

public class GameTester : MonoBehaviour {
    public List<Transform> boards = new List<Transform>();
    public string boardName = "Test_Board";
    // Start is called before the first frame update
    private void Start() {
        // Create BoardSO
        Chess.Board.BoardSO boardSO = ScriptableObject.CreateInstance<Chess.Board.BoardSO>();

        for (int w = 0; w < boards.Count; w++) {
            Dimension dimension = new Dimension(null);
            dimension.dimensionElements = CreateBoard(boards[w], w);
            boardSO.dimensions.Add(dimension);
        }

        // save the boardSO
        AssetDatabase.CreateAsset(boardSO, "Assets/Resources/Boards/"+boardName+".asset");
        AssetDatabase.SaveAssets();
    }

    private List<BoardElement> CreateBoard(Transform board, int w) {
        // get children pieces of board
        Transform[] boardPieces = board.GetComponentsInChildren<Transform>();
        boardPieces = boardPieces[1..];
        List<BoardElement> board_element_list = new List<BoardElement>();

        // round position of all board pieces and add to list
        foreach (Transform piece in boardPieces)
        {
            if(piece.tag == "#") {
                piece.localPosition = new Vector3(Mathf.Round(piece.localPosition.x), Mathf.Round(piece.localPosition.y), Mathf.Round(piece.localPosition.z));
                board_element_list.Add(new BoardElement(piece.tag[0], (int)piece.localPosition.x, (int)piece.localPosition.y, (int)piece.localPosition.z, w));
            } else if (piece.tag != "Untagged") {
                int yPos = (int) Mathf.Ceil(piece.localPosition.y);
                piece.localPosition = new Vector3(Mathf.Round(piece.localPosition.x), yPos - 0.5f, Mathf.Round(piece.localPosition.z));
                board_element_list.Add(new BoardElement(piece.tag[0], (int)piece.localPosition.x, yPos, (int)piece.localPosition.z, w));
            } else { // skip unknwon piece
                continue;
            }
        }

        return board_element_list;
    }
}