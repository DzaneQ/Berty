using Berty.Grid.Entities;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Berty.UI.Managers
{
    public class OverlayObjectManager : UIObjectManager<OverlayObjectManager>
    {
        public void DisplayGameOverScreen(AlignmentEnum winner)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/GameOver");
            Text endingMessage = prefab.transform.GetChild(0).gameObject.GetComponent<Text>();
            if (winner == AlignmentEnum.Player) endingMessage.text = LanguageManager.Instance.GetTextFromKey("win");
            else if (winner == AlignmentEnum.Opponent) endingMessage.text = LanguageManager.Instance.GetTextFromKey("lose");
            else throw new Exception("Undefined winner.");
            Instantiate(prefab, canvasObject.transform);
        }
    }
}
