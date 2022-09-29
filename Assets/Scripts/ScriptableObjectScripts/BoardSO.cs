namespace Chess.Board
{
    using System.Collections;
    using System.Collections.Generic;
    using System;
    using UnityEngine;

    [Serializable] public struct BoardPosition {
        public int x, y, z, w;
        public BoardPosition(int x, int y, int z, int w) {
            (this.x,this.y,this.z,this.w) = (x,y,z,w);
        }

        public BoardPosition(BoardPosition position) {
            (this.x,this.y,this.z,this.w) = (position.x,position.y,position.z,position.w);
        }

        public int this[int index] => index switch
        {
            0 => x,
            1 => y,
            2 => z,
            3 => w,
            _ => throw new IndexOutOfRangeException("BoardPosition get Index must be between 0 and 3"),
        };

        public BoardPosition add (BoardPosition other) {
            return new BoardPosition(this.x + other.x, this.y + other.y, this.z + other.z, this.w + other.w);
        }

        public void setValue(int index, int value) {
            switch (index)
            {
                case 0:
                    x = value;
                    break;
                case 1:
                    y = value;
                    break;
                case 2:
                    z = value;
                    break;
                case 3:
                    w = value;
                    break;
                default:
                    throw new IndexOutOfRangeException("BoardPosition setValue Index must be between 0 and 3");
            }
        }

    }

    [Serializable] public struct BoardElement
    {
        public char element_value;
        public BoardPosition position;

        // Constructor
        public BoardElement(char element_value, int x, int y, int z, int w)
        {
            this.element_value = element_value;
            position = new BoardPosition(x,y,z,w);
        }
    }

    [Serializable] public struct Dimension {
        public List<BoardElement> dimensionElements;
        public ColorThemeSO colorTheme;

        // Constructor
        public Dimension(ColorThemeSO colorTheme)
        {
            dimensionElements = new List<BoardElement>();
            this.colorTheme = colorTheme;
        }
    }

    [CreateAssetMenu(fileName = "ChessBoard", menuName = "Chess/Board")]
    public class BoardSO : ScriptableObject
    {
        public List<Dimension> dimensions = new List<Dimension>();
    }
}
