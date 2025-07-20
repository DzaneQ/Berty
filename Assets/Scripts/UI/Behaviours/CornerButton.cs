using Berty.Audio;
using Berty.Enums;
using Berty.Gameplay;
using Berty.Gameplay.Managers;
using Berty.UI.Card;
using Berty.UI.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Berty.UI.Listeners
{
    public class CornerButton : MonoBehaviour
    {
        private CornerButtonEnum _buttonType;
        public CornerButtonEnum ButtonType
        {
            get => _buttonType;
            private set
            {
                _buttonType = value;
                UpdateButtonLabel();
            }
        }
        private Text label;

        void Start()
        {
            _buttonType = CornerButtonEnum.EndTurn;
            label = transform.GetChild(0).GetComponent<Text>();
        }

        public void DisplayButton(CornerButtonEnum buttonType)
        {
            ButtonType = buttonType;
        }

        private void UpdateButtonLabel()
        {
            label.text = ButtonType switch
            {
                CornerButtonEnum.EndTurn => LanguageManager.Instance.GetTextFromKey("end_turn"),
                CornerButtonEnum.Undo => LanguageManager.Instance.GetTextFromKey("undo"),
                _ => throw new Exception("Unknown button type.")
            };
        }
    }
}
