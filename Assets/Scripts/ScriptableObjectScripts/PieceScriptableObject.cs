namespace Chess.Pieces
{
    using System.Collections;
    using System.Collections.Generic;
    using System;
    using UnityEngine;

    public enum PieceColor {
        Light = 1,
        Dark = -1
    }

    public abstract class Moveset
    {
        private Func<GameObject[,,,], (int,int,int,int), bool>[] conditions; //gets board and origin position, returns bool
        public abstract (int,int,int,int)[] GetMoveset();
    }

    public class StandardPawnMoveset : Moveset {
        public override (int,int,int,int)[] GetMoveset() {
            return new (int,int,int,int)[] {
                (0, 1, 0, 0),
                (0, 2, 0, 0),
                (1, 1, 0, 0),
                (-1, 1, 0, 0)
            };
        }
    }

    [CreateAssetMenu(fileName = "ChessPiece", menuName = "Chess/Piece")]
    public class PieceScriptableObject : ScriptableObject
    {
        public string piece_name;
        public PieceColor team;
        public Moveset[] movesets;
    }
}