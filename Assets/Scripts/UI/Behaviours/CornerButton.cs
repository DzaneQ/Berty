using Berty.Enums;
using Berty.Gameplay.Managers;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Berty.UI.Listeners
{
    public class CornerButton : MonoBehaviour // TODO: Refactor - name too generic, inside code might be simpler
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
            UpdateButtonLabel();
        }

        public void DisplayButton(CornerButtonEnum buttonType)
        {
            ButtonType = buttonType;
        }

        private void UpdateButtonLabel()
        {
            label.text = ButtonType switch
            {
                CornerButtonEnum.EndTurn => GameLanguageManager.Instance.GetTextFromKey("end_turn"),
                CornerButtonEnum.Undo => GameLanguageManager.Instance.GetTextFromKey("undo"),
                _ => throw new Exception("Unknown button type.")
            };
        }
    }
}
