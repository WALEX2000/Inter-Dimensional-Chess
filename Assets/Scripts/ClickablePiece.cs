namespace Chess.Pieces
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    using Chess.Game;

    public class ClickablePiece : MonoBehaviour
    {
        private bool white_team = true;
        private ClickablePieceStateMachine stateMachine;
        
        private void Start() {
            if(Char.IsLower(this.tag[0])) {
                white_team = false;
            }

            stateMachine = new ClickablePieceStateMachine(this.gameObject);    
        }

        private bool PlayingTurn() {
            return GameManager.Instance.IsWhiteTurn = white_team;
        }

        private void OnMouseEnter()
        { // when mouse enters the collider
            if(PlayingTurn()) {
                stateMachine.ApplyCommand(PieceCommand.PlayerEnter);
            } else {
                stateMachine.ApplyCommand(PieceCommand.EnemyEnter);
            }
        }

        private void OnMouseExit()
        { // Exit the hover state
            if(PlayingTurn()) {
                stateMachine.ApplyCommand(PieceCommand.PlayerLeave);
            } else {
                stateMachine.ApplyCommand(PieceCommand.EnemyLeave);
            }
        }

        private void OnMouseOver()
        { // Hover
        }

        private void OnMouseDown()
        { // Click on Piece
        }

        private void OnMouseDrag()
        { // Holding down Mouse button after clicking on piece
        }

        private void OnMouseUp()
        { // Release Click
        }

        private void OnMouseUpAsButton()
        { // Release Click inside collider
        }
    }

}