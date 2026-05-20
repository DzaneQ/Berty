using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Berty.Gameplay.Managers.Shared
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
}