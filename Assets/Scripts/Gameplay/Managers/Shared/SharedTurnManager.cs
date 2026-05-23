using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Utility;
using Unity.Netcode;
using UnityEngine;

namespace Berty.Gameplay.Managers.Shared
{
    public class SharedTurnManager : SharedManagerSingleton<SharedTurnManager>
    {
        private Game game;
        private NetworkVariable<AlignmentEnum> turnAlignment = new();

        private void Start()
        {
            game = EntityLoadManager.Instance.Game;
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                game ??= EntityLoadManager.Instance.Game;
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

        private void OnTurnAlignmentChanged(AlignmentEnum prv, AlignmentEnum crr)
        {
            SyncGameEntityToClients.Instance.Sync(); // NOTE: Probably remove because potentially unstable and may cause race condtion.
            EventManager.Instance.RaiseOnNewTurn(); // TODO: Update new alignment, either by passing it as an argument or reading NetworkVariable value.
            CheckpointManager.Instance.RequestCheckpoint();
        }

        [ServerRpc]
        private void SwitchAlignmentServerRpc()
        {
            turnAlignment.Value = game.SwitchAlignment();
        }
    }
}
