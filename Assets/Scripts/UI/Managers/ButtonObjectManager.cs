using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Entities;
using Berty.UI.Listeners;
using Berty.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Berty.UI.Managers
{
    public class ButtonObjectManager : UIObjectManager<ButtonObjectManager>
    {
        CornerButton cornerButton;

        protected override void Awake()
        {
            InitializeSingleton();
            cornerButton = ObjectReadManager.Instance.CornerButton.GetComponent<CornerButton>();
        }

        public void DisplayEndTurnButton()
        {
            cornerButton.gameObject.SetActive(true);
            cornerButton.DisplayButton(CornerButtonEnum.EndTurn);
        }

        public void DisplayUndoButton()
        {
            cornerButton.gameObject.SetActive(true);
            cornerButton.DisplayButton(CornerButtonEnum.Undo);
        }

        public void HideCornerButton()
        {
            cornerButton.gameObject.SetActive(false);
        }
    }
}
