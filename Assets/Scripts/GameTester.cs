using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if(piece.tag == "Block") {
                piece.position = new Vector3(Mathf.Round(piece.position.x), Mathf.Round(piece.position.y), Mathf.Round(piece.position.z));
                boardMatrix[(int)piece.localPosition.x, (int)piece.localPosition.y, (int)piece.localPosition.z] = 'B';
            } else if (piece.tag == "White Piece" || piece.tag == "Black Piece") {
                int yPos = (int) Mathf.Ceil(piece.position.y);
                piece.position = new Vector3(Mathf.Round(piece.position.x), yPos - 0.5f, Mathf.Round(piece.position.z));
                boardMatrix[(int)piece.localPosition.x, yPos, (int)piece.localPosition.z] = 'P';
            }
        }
        Debug.Log("Started!");
    }

    public void ClickedOnPiece(int x, int y, int z)
    {
        Debug.Log("Clicked on piece at " + x + ", " + y + ", " + z);
        Debug.Log("Found: " + boardMatrix[x, y, z]);
    }
}