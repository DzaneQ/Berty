using Berty.Entities;
using Berty.Enums;
using Berty.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Berty.Managers
{
    public class GameObjectManager : ManagerSingleton<GameObjectManager>
    {
        private GameObject canvasObject;

        protected override void Awake()
        {
            InitializeSingleton();
            canvasObject = FindObjectOfType<Canvas>().gameObject;
        }

        public void DisplayGameOverScreen(Alignment winner)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/GameOver");
            Text endingMessage = prefab.transform.GetChild(0).gameObject.GetComponent<Text>();
            if (winner == Alignment.Player) endingMessage.text = LanguageManager.Instance.GetTextFromKey("win");
            else if (winner == Alignment.Opponent) endingMessage.text = LanguageManager.Instance.GetTextFromKey("lose");
            else throw new Exception("Undefined winner.");
            Instantiate(prefab, canvasObject.transform);
        }
    }
}
