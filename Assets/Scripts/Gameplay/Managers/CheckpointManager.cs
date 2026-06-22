using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Utility;
using System;

namespace Berty.Gameplay.Managers
{
    public class CheckpointManager : ManagerSingleton<CheckpointManager>
    {
        private Game game;
        private bool requestedCheckpoint;

        protected override void Awake()
        {
            base.Awake();
            game = EntityLoadManager.Instance.Game;
        }

        public void RequestCheckpoint()
        {
            if (requestedCheckpoint) throw new Exception("Trying to request checkpoint when the previous request has not been handled.");
            if (CanHandleCheckpoint()) HandleCheckpoint();
            else requestedCheckpoint = true;
        }

        public void HandleIfRequested()
        {
            if (!requestedCheckpoint) return;
            if (CanHandleCheckpoint()) HandleCheckpoint();
        }

        private void HandleCheckpoint()
        {
            if (!TryEndingTheGame()) SaveTheGame();
            requestedCheckpoint = false;
        }

        private bool CanHandleCheckpoint()
        {
            return !EventManager.Instance.RaiseOnCheckpointRequest();
        }

        private bool TryEndingTheGame()
        {
            int alignedCardsToWin = game.GameConfig.AlignedCardsToWin;
            if (game.Grid.AlignedFields(AlignmentEnum.Player, true).Count >= alignedCardsToWin
                || game.Grid.AlignedFields(AlignmentEnum.Opponent, true).Count >= alignedCardsToWin)
            {
                EndTheGame();
                return true;
            }
            return false;
        }

        private void EndTheGame()
        {
            ManagerLocator.TurnManagerInstance.EndTheGame();
            //LoadSaveManager.Instance.DeleteTheSave();
        }

        private void SaveTheGame()
        {
            ProcessGameDataManager.Instance.SaveTheGame();
        }
    }
}
