using Assets.Scripts.UI.Listeners;
using Berty.Audio.Managers;
using Berty.Enums;
using Berty.UI.Managers;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Berty.UI.Listeners
{
    public class ButtonInput : MonoBehaviour, IPointerUpHandler
    {
        private ButtonDisplay _buttonDisplay;

        private CornerButtonEnum ButtonType => _buttonDisplay.ButtonType;

        private void Awake()
        {
            _buttonDisplay = GetComponent<ButtonDisplay>();
            if (_buttonDisplay == null) throw new Exception("ButtonDisplay component not found in" + name);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject == transform.GetChild(0).gameObject) HandleTheButtonClick();
        }

        private void HandleTheButtonClick()
        {
            SoundManager.Instance.ButtonClickSound();
            ButtonActionManager.Instance.HandleCornerButtonClick(ButtonType);
        }
    }
}
