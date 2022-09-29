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
        private PieceState state;
        private Material idle_material;
        public Material hover_materail, selected_materail; // TODO: Move materials to either a SO or the ColorTheme SO
        
        private void Start() {
            if(Char.IsLower(this.tag[0])) {
                white_team = false;
            }
            state = PieceState.Idle;
            idle_material = this.GetComponent<MeshRenderer>().material;
        }

        private bool PlayingTurn() {
            return GameManager.Instance.isWhiteTurn == white_team;
        }

        private void OnMouseEnter()
        { // when mouse enters the collider
            // If mouse is button is already down, then return
            if(Input.GetMouseButton(0)) return;
            PieceCommand command = PieceCommand.EnemyEnter;
            if(PlayingTurn()) {
                command = PieceCommand.PlayerEnter;
            }
            ExecuteState(ClickablePieceStateMachine.ApplyCommand(state, command));
        }

        private void OnMouseOver()
        { // Hovering
            if(Input.GetMouseButton(0)) return;
            PieceCommand command = PieceCommand.EnemyHover;
            if(PlayingTurn()) {
                command = PieceCommand.PlayerHover;
            }
            ExecuteState(ClickablePieceStateMachine.ApplyCommand(state, command));
        }

        private void OnMouseExit()
        { // Exit the hover state
            PieceCommand command = PieceCommand.EnemyLeave;
            if(PlayingTurn()) {
                command = PieceCommand.PlayerLeave;
            }
            ExecuteState(ClickablePieceStateMachine.ApplyCommand(state, command));
        }

        private void OnMouseDown()
        { // Click on Piece
            PieceCommand command = PieceCommand.EnemyClick;
            if(PlayingTurn()) {
                command = PieceCommand.PlayerClick;
            }
            ExecuteState(ClickablePieceStateMachine.ApplyCommand(state, command));
        }

        private void OnMouseDrag()
        { // Holding down Mouse button after clicking on piece
            //Move the piece and highlight which block it would fall into if released
        }

        private void OnMouseUp()
        { // Release Click
            // If the piece is in the correct position, then release it
        }

        private void OnMouseUpAsButton()
        { // Release Click inside collider
        }

        public void Deselect() { // Call when piece should be deselected
            ExecuteState(ClickablePieceStateMachine.ApplyCommand(state, PieceCommand.Deselect));
        }

        private void ExecuteState(PieceState? nextState) {
            if(nextState == null) return;
            else state = nextState.Value;
            switch(state) {
                case PieceState.Idle:
                    EnterIdle();
                    break;
                case PieceState.Hovered:
                    EnterHovered();
                    break;
                case PieceState.Selected:
                    EnterSelected();
                    break;
                case PieceState.Played:
                    EnterPlayed();
                    break;
            }
        }

        private void EnterIdle() {
            // TODO: Change color to default
            this.GetComponent<MeshRenderer>().material = idle_material;
        }

        private void EnterHovered() {
            // TODO: Add outline glow effect
            this.GetComponent<MeshRenderer>().material = hover_materail;
        }

        private void EnterSelected() {
            GameManager.Instance.SelectPiece(this);
            this.GetComponent<MeshRenderer>().material = selected_materail;
        }

        private void EnterPlayed() {
            // TODO: Change piece position (Maybe I don't need this state)
            // Remove Selection effects (Go back to idle looks)
            // End Turn
        }
    }

}