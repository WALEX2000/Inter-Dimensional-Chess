namespace Chess.Board
{
    using System.Collections;
    using System.Collections.Generic;
    using System;
    using UnityEngine;

    public class BoardSO : ScriptableObject
    {
        private GameObject[,,,] boardMatrix = new GameObject[12, 12, 12, 6];
    }
}
