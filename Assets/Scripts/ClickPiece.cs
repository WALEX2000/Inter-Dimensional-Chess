using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickPiece : MonoBehaviour
{
    public GameTester gameTester;

    private void OnMouseDown() {
        Debug.Log("Clicked");
        gameTester.ClickedOnPiece((int)transform.localPosition.x, (int)Mathf.Ceil(transform.localPosition.y), (int)transform.localPosition.z);
    }
}
