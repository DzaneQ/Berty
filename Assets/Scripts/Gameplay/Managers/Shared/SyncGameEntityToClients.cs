using Berty.Gameplay.Entities;
using Berty.Utility;
using Unity.Netcode;

namespace Berty.Gameplay.Managers.Shared
{
    public class SyncGameEntityToClients : SharedManagerSingleton<SyncGameEntityToClients>
    {
        public void Sync()
        {
            string gameDataStr = ProcessGameDataManager.Instance.GetGameEntityAsString();
            SyncGameEntityClientRpc(gameDataStr);
        }

        [ClientRpc]
        private void SyncGameEntityClientRpc(string gameDataStr)
        {
            GameSaveData data = ProcessGameDataManager.Instance.GetDataFromString(gameDataStr);
            EntityLoadManager.Instance.LoadGameFromData(data);
        }
    }
}