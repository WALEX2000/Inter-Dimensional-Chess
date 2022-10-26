namespace Chess.Board
{
    using System.Collections;
    using System.Collections.Generic;
    using System;
    using UnityEngine;

    public enum BoardAxis { X, Y, Z, W };

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

        public bool IsEqual(BoardPosition other) {
            return (other.x == this.x && other.y == this.y && other.z == this.z && other.w == this.w);
        }

        public BoardPosition Add (BoardPosition other) {
            return new BoardPosition(this.x + other.x, this.y + other.y, this.z + other.z, this.w + other.w);
        }

        public int DistanceBetween (BoardPosition other) {
            return Math.Abs(this.x - other.x) + Math.Abs(this.y - other.y) + Math.Abs(this.z - other.z) + Math.Abs(this.w - other.w);
        }

        public void SetValue(int index, int value) {
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
        public int minDimensionRank, maxDimensionRank;

        // Constructor
        public Dimension(ColorThemeSO colorTheme)
        {
            dimensionElements = new List<BoardElement>();
            this.colorTheme = colorTheme;
            minDimensionRank = 0;
            maxDimensionRank = 0;
        }

        public void setMinDimensionRank(int minDimensionRank) {
            this.minDimensionRank = minDimensionRank;
        }

        public void setMaxDimensionRank(int maxDimensionRank) {
            this.maxDimensionRank = maxDimensionRank;
        }
    }

    [CreateAssetMenu(fileName = "ChessBoard", menuName = "Chess/Board")]
    public class BoardSO : ScriptableObject
    {
        public List<Dimension> dimensions = new List<Dimension>();
        public (int,int,int,int) GetMaxBoardSize()
        { // TODO: precalculate this, to optimize
            int maxX = 0, maxY = 0, maxZ = 0, maxW = dimensions.Count;
            foreach (Dimension dimension in dimensions)
            {
                foreach (BoardElement element in dimension.dimensionElements)
                {
                    maxX = Math.Max(maxX, element.position.x);
                    maxY = Math.Max(maxY, element.position.y);
                    maxZ = Math.Max(maxZ, element.position.z);
                }
            }
            maxX+=2; maxY+=2; maxZ+=2;
            return (maxX,maxY,maxZ,maxW);
        }
    }
}
