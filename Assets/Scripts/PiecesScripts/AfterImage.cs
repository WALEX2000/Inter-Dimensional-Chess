namespace Chess.Pieces
{
    using UnityEngine;
    using Chess.Board;
    
    public class AfterImage {
        public GameObject afterImageOwner {get;}
        public BoardPosition afterImagePosition {get;}
        public AfterImage(GameObject afterImageOwner, BoardPosition afterImagePosition) {
            this.afterImageOwner = afterImageOwner;
            this.afterImagePosition = afterImagePosition;
        }
    }
}
