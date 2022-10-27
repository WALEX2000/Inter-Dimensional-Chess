namespace Chess.Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Chess.Board;
    using Chess.Interface;
    using System;
    using UnityEngine.Events;

    public class GameManager : MonoBehaviour
    {
        // Singleton
        private static GameManager _instance;
        public static GameManager Instance { 
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<GameManager>();
                    if(_instance == null) {
                        throw new System.Exception("Trying to access Chess.Game.GameManager but it does not exist in the Scene!");
                    }
                }
                return _instance;
            } 
        }
        private void Awake() {
            _instance = this;
        }

        // Constants
        public const string GUIHolderObjectName = "GUIHolder";

        public ChessBoard gameBoard;
        public Transform boardTransform;

        public BoardInterface boardInterface;

        public void CreateBoard((int,int,int,int) boardSize) {
            gameBoard = new ChessBoard(boardSize.Item1, boardSize.Item2, boardSize.Item3, boardSize.Item4);
        }

        // Game Function Methods
        public bool isWhiteTurn = true;
        public void StartTurn() {
            gameBoard.CalculateTeamMoves(isWhiteTurn);
            boardInterface.DisplayBoard();
        }

        public void EndTurn() {
            isWhiteTurn = !isWhiteTurn;
            StartTurn();
        }
    }
}