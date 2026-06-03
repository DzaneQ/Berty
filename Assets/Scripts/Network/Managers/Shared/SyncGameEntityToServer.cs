/*using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Utility;
using Unity.Netcode;

namespace Berty.Network.Managers.Shared
{
    public class SyncGameEntityToServer : SharedManagerSingleton<SyncGameEntityToServer>
    {
        public void Sync()
        {
            string gameDataStr = ProcessGameDataManager.Instance.GetGameEntityAsString();
            SyncGameEntityServerRpc(gameDataStr);
        }

        [ServerRpc]
        private void SyncGameEntityServerRpc(string gameDataStr)
        {
            GameSaveData data = ProcessGameDataManager.Instance.GetDataFromString(gameDataStr);
            EntityLoadManager.Instance.LoadGameFromData(data);
        }
    }
}*/