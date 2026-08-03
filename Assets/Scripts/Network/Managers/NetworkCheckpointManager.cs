using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Utility;
using System;
using Unity.Netcode;
using UnityEngine;

namespace Berty.Network.Managers
{
    public class NetworkCheckpointManager : RpcManagerSingleton<NetworkCheckpointManager>, ICheckpointManager
    {
        private Game game;
        private bool requestedCheckpoint;
        private StatusEnum[] selectionStatuses;

        public override void OnNetworkSpawn()
        {
            game = EntityLoadManager.Instance.Game;
            selectionStatuses = new StatusEnum[] { StatusEnum.ClickToApplyEffect, StatusEnum.RevivalSelect };
        }

        // BUG: KrolPopuBert after dying will leave an exception on turn end
        public void RequestCheckpoint()
        {
            if (requestedCheckpoint) throw new Exception("Trying to request checkpoint when the previous request has not been handled.");
            if (IsStatusPreventingCheckpoint())
            {
                requestedCheckpoint = false; // Cancel checkpoint request
                return;
            }
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
            TryEndingTheGameServerRpc();
            requestedCheckpoint = false;
        }

        private bool CanHandleCheckpoint()
        {
            return !EventManager.Instance.RaiseOnCheckpointRequest();
        }

        private bool IsStatusPreventingCheckpoint()
        {
            return game.AreThereAnyStatuses(selectionStatuses);
        }

        [ServerRpc(RequireOwnership = false)]
        private void TryEndingTheGameServerRpc()
        {
            int alignedCardsToWin = game.GameConfig.AlignedCardsToWin;
            if (game.Grid.AlignedFields(AlignmentEnum.Player, true).Count >= alignedCardsToWin
                || game.Grid.AlignedFields(AlignmentEnum.Opponent, true).Count >= alignedCardsToWin) EndTheGame();
        }

        private void EndTheGame()
        {
            ManagerLocator.TurnManagerInstance.EndTheGame();
        }
    }
}
