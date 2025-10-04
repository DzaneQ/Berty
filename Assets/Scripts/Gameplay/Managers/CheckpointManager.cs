using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Grid.Entities;
using Berty.UI.Card.Managers;
using Berty.UI.Managers;
using Berty.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Gameplay.Managers
{
    public class CheckpointManager : ManagerSingleton<CheckpointManager>
    {
        private Game game;
        private bool requestedCheckpoint;

        protected override void Awake()
        {
            base.Awake();
            game = CoreManager.Instance.Game;
        }

        public void RequestCheckpoint()
        {
            if (requestedCheckpoint) throw new Exception("Trying to request checkpoint when the previous request has not been handled.");
            if (CanHandleCheckpoint()) HandleCheckpoint();
            else requestedCheckpoint = true;
        }

        // BUG: Does not end the game when side changed into card count required to win
        public void HandleIfRequested()
        {
            if (!requestedCheckpoint) return;
            if (CanHandleCheckpoint()) HandleCheckpoint();
        }   

        private void HandleCheckpoint()
        {
            Debug.Log("Handling checkpoint");
            TryEndingTheGame();
            requestedCheckpoint = false;
        }

        private bool CanHandleCheckpoint()
        {
            return !EventManager.Instance.RaiseOnCheckpointRequest();
        }

        private void TryEndingTheGame()
        {
            int alignedCardsToWin = game.GameConfig.AlignedCardsToWin;
            if (game.Grid.AlignedFields(AlignmentEnum.Player, true).Count >= alignedCardsToWin
                || game.Grid.AlignedFields(AlignmentEnum.Opponent, true).Count >= alignedCardsToWin)
                EndTheGame();
        }

        private void EndTheGame()
        {
            TurnManager.Instance.EndTheGame();
        }
    }
}
