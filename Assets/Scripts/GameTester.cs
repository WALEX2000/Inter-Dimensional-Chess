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

        // TODO: Create board min/max boundaries and add to boardSO when doing this for real
        int minX = Int32.MaxValue, maxX = 0, minY = Int32.MaxValue, maxY = 0, minZ = Int32.MaxValue, maxZ = 0;
        for (int w = 0; w < boards.Count; w++) {
            Dimension dimension = new Dimension(null);
            CreateBoard(boards[w], w, ref dimension);
            boardSO.dimensions.Add(dimension);
        }

        // save the boardSO
        AssetDatabase.CreateAsset(boardSO, "Assets/Resources/Boards/"+boardName+".asset");
        AssetDatabase.SaveAssets();
    }

    private void CreateBoard(Transform board, int w, ref Dimension dimension) {
        // get children pieces of board
        Transform[] boardPieces = board.GetComponentsInChildren<Transform>();
        boardPieces = boardPieces[1..];
        List<BoardElement> boardElementList = dimension.dimensionElements;

        // round position of all board pieces and add to list
        int minZ = Int32.MaxValue, maxZ = 0;
        foreach (Transform piece in boardPieces)
        {
            if(piece.tag == "#") {
                piece.localPosition = new Vector3(Mathf.Round(piece.localPosition.x), Mathf.Round(piece.localPosition.y), Mathf.Round(piece.localPosition.z));
                int xPos = (int)piece.localPosition.x, yPos = (int)piece.localPosition.y, zPos = (int)piece.localPosition.z;
                boardElementList.Add(new BoardElement(piece.tag[0], xPos, yPos, zPos, w));
            } else if (piece.tag != "Untagged") {
                int yPos = (int) Mathf.Ceil(piece.localPosition.y);
                piece.localPosition = new Vector3(Mathf.Round(piece.localPosition.x), yPos - 0.5f, Mathf.Round(piece.localPosition.z));
                boardElementList.Add(new BoardElement(piece.tag[0], (int)piece.localPosition.x, yPos, (int)piece.localPosition.z, w));
            } else { // skip unknwon piece
                continue;
            }

            // update min/max values
            minZ = Math.Min(minZ, (int)piece.localPosition.z);
            maxZ = Math.Max(maxZ, (int)piece.localPosition.z);
        }

        dimension.setMinDimensionRank(minZ);
        dimension.setMaxDimensionRank(maxZ);
    }
}