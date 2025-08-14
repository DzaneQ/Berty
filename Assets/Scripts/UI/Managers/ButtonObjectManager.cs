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
        private CornerButton cornerButton;
        private Game game;

        protected override void Awake()
        {
            InitializeSingleton();
            cornerButton = ObjectReadManager.Instance.CornerButton.GetComponent<CornerButton>();
            game = CoreManager.Instance.Game;
        }

        public void DisplayEndTurnButton()
        {
            cornerButton.DisplayButton(CornerButtonEnum.EndTurn);
            // Don't set active if opponent's turn
            cornerButton.gameObject.SetActive(true);
        }

        public void DisplayUndoButton()
        {
            cornerButton.DisplayButton(CornerButtonEnum.Undo);
            // Don't set active if opponent's turn
            cornerButton.gameObject.SetActive(true);
        }

        public void HideCornerButton()
        {
            cornerButton.gameObject.SetActive(false);
        }
    }
}
