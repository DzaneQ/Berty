using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Utility;
using System;
using Unity.Netcode;
using UnityEngine;

namespace Berty.Network.Managers
{
    public class NetworkTurnManager : SharedManagerSingleton<NetworkTurnManager>, ITurnManager
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
            Debug.Log("Clicked end turn");
            SwitchAlignmentServerRpc();
        }

        public void EndTheGame()
        {
            throw new NotImplementedException("EndTheGame should be implemented for multiplayer.");
        }

        public bool IsItMyTurn()
        {
            return PlayerReadManager.Instance.MyAlignment == CurrentAlignment;
        }

        private void OnTurnAlignmentChanged(AlignmentEnum prv, AlignmentEnum crr)
        {
            if (prv == crr) throw new Exception($"Turn alignment should not be the same after change: {prv}");
            Debug.Log($"Turn alignment changed from {prv} to {crr}");
            EventManager.Instance.RaiseOnNewTurn();
            CheckpointManager.Instance.RequestCheckpoint();
        }

        [ServerRpc(RequireOwnership = false)]
        private void SwitchAlignmentServerRpc()
        {
            turnAlignment.Value = game.SwitchAlignment();
        }
    }
}
