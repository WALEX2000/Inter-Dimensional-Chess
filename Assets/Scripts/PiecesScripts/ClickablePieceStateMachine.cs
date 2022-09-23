namespace Chess.Pieces
{
    using System.Collections.Generic;
    using UnityEngine;

    public enum PieceState { // TODO: add Drag support
        Idle,
        Hovered,
        Selected,
        Played
    }

    public enum PieceCommand {
        PlayerEnter,
        PlayerHover,
        PlayerLeave,
        EnemyEnter,
        EnemyHover,
        EnemyLeave,
        PlayerClick,
        EnemyClick,
        Deselect,
    }

    public static class ClickablePieceStateMachine
    {
        private static Dictionary<PieceCommand, PieceState>[] stateTransitions = new Dictionary<PieceCommand, PieceState>[] {
            new Dictionary<PieceCommand, PieceState>() { // Idle
                { PieceCommand.PlayerEnter, PieceState.Hovered },
                { PieceCommand.PlayerHover, PieceState.Hovered },
                { PieceCommand.PlayerClick, PieceState.Selected },
            },
            new Dictionary<PieceCommand, PieceState>() { // Hovered
                { PieceCommand.PlayerLeave, PieceState.Idle },
                { PieceCommand.PlayerClick, PieceState.Selected },
            },
            new Dictionary<PieceCommand, PieceState>() { // Selected
                { PieceCommand.Deselect, PieceState.Idle },
            },
            new Dictionary<PieceCommand, PieceState>() { // Played

            },
        };

        public static PieceState? ApplyCommand(PieceState state, PieceCommand command) {
            if(!stateTransitions[(int)state].TryGetValue(command, out PieceState nextState)) {
                return null;  // Transition doesn't exist, so return null
            } else {
                return nextState;
            }            
        }
    }
}
