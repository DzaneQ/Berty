/*using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Utility;
using Unity.Netcode;
using UnityEngine;

namespace Berty.Network.Managers.Shared
{
    public class SyncGameEntityToClients : SharedManagerSingleton<SyncGameEntityToClients>
    {
        public void Sync()
        {
            if (!NetworkManager.Singleton.IsServer) return;
            string gameDataStr = ProcessGameDataManager.Instance.GetGameEntityAsString();
            Debug.Log($"Syncing before rpc: {EntityLoadManager.Instance.Game.CurrentAlignment}");
            SyncGameEntityClientRpc(gameDataStr);
        }

        [ClientRpc]
        private void SyncGameEntityClientRpc(string gameDataStr)
        {
            Debug.Log($"Syncing within rpc: {EntityLoadManager.Instance.Game.CurrentAlignment}");
            GameSaveData data = ProcessGameDataManager.Instance.GetDataFromString(gameDataStr);
            EntityLoadManager.Instance.LoadGameFromData(data);
        }
    }
}*/