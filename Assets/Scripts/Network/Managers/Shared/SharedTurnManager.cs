using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Gameplay.Managers.Client;
using Berty.Utility;
using System;
using Unity.Netcode;
using UnityEngine;

namespace Berty.Network.Managers.Shared
{
    public class SharedTurnManager : SharedManagerSingleton<SharedTurnManager>, ITurnManager
    {
        private Game game;
        private readonly NetworkVariable<AlignmentEnum> turnAlignment = new();

        public AlignmentEnum CurrentAlignment => turnAlignment.Value;

        private void Start()
        {
            game = EntityLoadManager.Instance.Game;
        }

        public override void OnNetworkSpawn()
        {
            game ??= EntityLoadManager.Instance.Game;
            if (IsServer)
            {
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
            if (game.CurrentAlignment != crr) game.SwitchAlignment();
            //SyncGameEntityToClients.Instance.Sync(); // NOTE: Probably remove because potentially unstable and may cause race condtion.
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
