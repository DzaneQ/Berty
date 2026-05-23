using Berty.Audio;
using Berty.Audio.Managers;
using Berty.Enums;
using Berty.Gameplay;
using Berty.Gameplay.Managers;
using Berty.UI.Card;
using Berty.UI.Managers;
using Berty.UI.Managers.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Berty.UI.Listeners.Client
{
    public class ClientButtonInput : MonoBehaviour, IPointerUpHandler
    {
        private CornerButton core;

        void Awake()
        {
            core = GetComponent<CornerButton>();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (Input.GetMouseButtonUp(0) && eventData.pointerCurrentRaycast.gameObject == transform.GetChild(0).gameObject) HandleTheButtonClick();
        }

        private void HandleTheButtonClick()
        {
            SoundManager.Instance.ButtonClickSound();
            ClientButtonActionManager.Instance.HandleCornerButtonClick(core.ButtonType);
        }
    }
}
