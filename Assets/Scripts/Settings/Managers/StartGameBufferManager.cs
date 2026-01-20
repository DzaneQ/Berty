using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Settings
{
    public class StartGameBufferManager : PersistentManagerSingleton<StartGameBufferManager>
    {
        public GameSaveData? Data { get; private set; }

        public void SetLoading(bool isLoading)
        {
            if (isLoading) Data = SaveLoadManager.Instance.LoadTheSaveFile();
            else Data = null;
        }

        public bool IsStartingNewGame()
        {
            return Data == null;
        }
    }
}
