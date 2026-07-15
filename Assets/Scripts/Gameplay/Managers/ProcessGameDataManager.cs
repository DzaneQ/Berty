using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Grid.Entities;
using Berty.UI.Card.Managers;
using Berty.UI.Managers;
using Berty.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Berty.Gameplay.Managers
{
    public class ProcessGameDataManager : ManagerSingleton<ProcessGameDataManager>
    {
        private Game game;
        private string savePath;

        protected override void Awake()
        {
            base.Awake();
            game = EntityLoadManager.Instance.Game;
            savePath = Application.persistentDataPath + "/save.txt"; // Change extension
        }

        public GameSaveData? LoadTheSaveFile() // BUG: Loading the game pulls extra hand cards and fails to load sprites
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
            return JsonUtility.ToJson(game.SaveEntity(), true);
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
