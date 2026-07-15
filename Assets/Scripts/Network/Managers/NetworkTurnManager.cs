using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Managers;
using Berty.Utility;
using System;
using Unity.Netcode;
using UnityEngine;

namespace Berty.Network.Managers
{
    public class NetworkTurnManager : RpcManagerSingleton<NetworkTurnManager>, ITurnManager
    {
        private Game game; // should be used from server only
        private readonly NetworkVariable<AlignmentEnum> turnAlignment = new();

        public AlignmentEnum CurrentAlignment => turnAlignment.Value;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                game = EntityLoadManager.Instance.Game;
                turnAlignment.Value = game.CurrentAlignment;
            }
            turnAlignment.OnValueChanged += OnTurnAlignmentChanged;
        }

        public override void OnNetworkDespawn()
        {
            turnAlignment.OnValueChanged -= OnTurnAlignmentChanged;
        }

        public void EndTurn()
        {
            SwitchAlignmentServerRpc();
        }

        public void EndTheGame()
        {
            if (!IsServer) throw new InvalidOperationException("Ending the game should be processed in the server.");
            AlignmentEnum winner = game.Grid.WinningSide();
            if (winner == AlignmentEnum.None) winner = CurrentAlignment;
            EndTheGameClientRpc(winner);
        }

        public bool IsItMyTurn()
        {
            return PlayerReadManager.Instance.MyAlignment == CurrentAlignment;
        }

        private void OnTurnAlignmentChanged(AlignmentEnum prv, AlignmentEnum crr)
        {
            if (prv == crr) throw new Exception($"Turn alignment should not be the same after change: {prv}");
            EventManager.Instance.RaiseOnNewTurn();
            ManagerLocator.CheckpointManagerInstance.RequestCheckpoint();
        }

        [ServerRpc(RequireOwnership = false)]
        private void SwitchAlignmentServerRpc()
        {
            turnAlignment.Value = game.SwitchAlignment();
        }

        [ClientRpc]
        private void EndTheGameClientRpc(AlignmentEnum winner)
        {
            if (winner == AlignmentEnum.None) throw new InvalidOperationException("Winner should not be None when ending the game");
            OverlayObjectManager.Instance.DisplayGameOverScreen(winner == PlayerReadManager.Instance.MyAlignment);
        }
    }
}
