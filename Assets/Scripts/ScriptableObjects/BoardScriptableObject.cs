namespace Chess.Board
{
    using System.Collections;
    using System.Collections.Generic;
    using System;
    using UnityEngine;

    [System.Serializable]
    public struct BoardElement
    {
        public char element_value;
        public int x;
        public int y;
        public int z;
        public int w;

        // Constructor
        public BoardElement(char element_value, int x, int y, int z, int w)
        {
            this.element_value = element_value;
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }

    [CreateAssetMenu(fileName = "ChessBoard", menuName = "Chess/Board")]
    public class BoardScriptableObject : ScriptableObject
    {
        public List<BoardElement> board_elem_list = new List<BoardElement>();
        public List<ColorThemeScriptableObject> color_theme_list = new List<ColorThemeScriptableObject>();
        public string board_name;
    }
}
