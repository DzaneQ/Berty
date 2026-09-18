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
        private bool requestedCheckpoint;
        private StatusEnum[] selectionStatuses;

        public void Awake()
        {
            selectionStatuses = new StatusEnum[] { StatusEnum.ClickToApplyEffect, StatusEnum.RevivalSelect };
        }

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
            return Game.AreThereAnyStatuses(selectionStatuses);
        }

        [Rpc(SendTo.Server)]
        private void TryEndingTheGameServerRpc()
        {
            int alignedCardsToWin = Game.GameConfig.AlignedCardsToWin;
            if (Game.Grid.AlignedFields(AlignmentEnum.Player, true).Count >= alignedCardsToWin
                || Game.Grid.AlignedFields(AlignmentEnum.Opponent, true).Count >= alignedCardsToWin) EndTheGame();
        }

        private void EndTheGame()
        {
            ManagerLocator.TurnManagerInstance.EndTheGame();
        }
    }
}
