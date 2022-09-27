namespace Chess.Board
{
    using System.Collections;
    using System.Collections.Generic;
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "ColorTheme", menuName = "Chess/Theme")]
    public class ColorThemeSO : ScriptableObject
    {
        public Material light_block_mat;
        public Material dark_block_mat;
        public Material light_piece_mat;
        public Material dark_piece_mat;
    }
}
