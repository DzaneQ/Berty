using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Utility;

namespace Berty.Settings
{
    public class StartGameBufferManager : PersistentManagerSingleton<StartGameBufferManager>
    {
        public GameSaveData? Data { get; private set; }

        public void SetLoading(bool isLoading)
        {
            if (isLoading) Data = ProcessGameDataManager.Instance.LoadTheSaveFile();
            else Data = null;
        }

        public bool IsStartingNewGame()
        {
            return Data == null;
        }
    }
}
