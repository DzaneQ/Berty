using Berty.Gameplay.Entities;
using Berty.Utility;
using System.IO;
using UnityEngine;

namespace Berty.Gameplay.Managers
{
    public class ProcessGameDataManager : ManagerSingleton<ProcessGameDataManager>
    {
        private Game _game;
        private string savePath;

        protected override void Awake()
        {
            base.Awake();
            savePath = Application.persistentDataPath + "/save.txt"; // Change extension
        }

        public GameSaveData? LoadTheSaveFile()
        {
            if (IsSaveFileExisting())
            {
                string saveContent = File.ReadAllText(savePath);
                return JsonUtility.FromJson<GameSaveData>(saveContent);
            }
            else return null;
        }

        public void SaveTheGame()
        {
            File.WriteAllText(savePath, GetGameEntityAsString());
        }

        public void DeleteTheSave()
        {
            if (IsSaveFileExisting()) File.Delete(savePath);
        }

        public string GetGameEntityAsString()
        {
            if (_game == null) _game = EntityLoadManager.Instance.Game;
            return JsonUtility.ToJson(_game.SaveEntity(), true);
        }

        public GameSaveData GetDataFromString(string dataStr)
        {
            return JsonUtility.FromJson<GameSaveData>(dataStr);
        }

        public bool IsSaveFileExisting()
        {
            return File.Exists(savePath);
        }
    }
}
