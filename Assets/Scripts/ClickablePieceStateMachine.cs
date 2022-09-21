namespace Chess.Pieces
{
    using System.Collections;
    using System.Collections.Generic;
    using System;
    using UnityEngine;

    enum PieceState { // TODO: add Drag support
        Idle,
        Hovered,
        Selected,
        Played
    }

    public enum PieceCommand {
        PlayerEnter,
        PlayerLeave,
        EnemyEnter,
        EnemyLeave,
    }

    public class ClickablePieceStateMachine
    {
        private PieceState state = PieceState.Idle;
        private List<Dictionary<PieceCommand, PieceState>> stateTransitions = new List<Dictionary<PieceCommand, PieceState>>();
        private GameObject piece;
        // Constructor
        public ClickablePieceStateMachine(GameObject piece)
        {
            this.piece = piece;

            // Create Idle transitions
            stateTransitions.Add(new Dictionary<PieceCommand, PieceState>(){
                {PieceCommand.PlayerEnter, PieceState.Hovered},
            });

            // Create Hovered transitions
            stateTransitions.Add(new Dictionary<PieceCommand, PieceState>(){
                {PieceCommand.PlayerLeave, PieceState.Idle},
            });

            // Create Selected transitions
            stateTransitions.Add(new Dictionary<PieceCommand, PieceState>(){
            });

            // Create Played transitions
            stateTransitions.Add(new Dictionary<PieceCommand, PieceState>(){
            });

        }

        public void ApplyCommand(PieceCommand command) {
            if(!stateTransitions[(int)state].TryGetValue(command, out PieceState nextState)) {
                return;  // Transition doesn't exist, so do nothing
            } else {
                state = nextState;
                ExecuteState();
            }            
        }

        private void ExecuteState() {
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
        }

        private void EnterHovered() {
            // TODO: Add outline glow effect
        }

        private void EnterSelected() {
            // TODO: Show possible moves
        }

        private void EnterPlayed() {
            // TODO: Change piece position
            // End Turn
        }
    }
}
