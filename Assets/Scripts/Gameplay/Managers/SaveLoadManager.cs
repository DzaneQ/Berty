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
    public class SaveLoadManager : ManagerSingleton<SaveLoadManager>
    {
        private Game game;
        private string savePath;

        protected override void Awake()
        {
            base.Awake();
            game = EntityLoadManager.Instance.Game;
            savePath = Application.persistentDataPath + "/save.txt"; // Change extension
            //Debug.Log("Save path:" + savePath);
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
            File.WriteAllText(savePath, JsonUtility.ToJson(game.SaveEntity(), true));
        }

        public bool IsSaveFileExisting()
        {
            return File.Exists(savePath);
        }
    }
}
