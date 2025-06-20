using Berty.CardTransfer.Managers;
using Berty.Display.Managers;
using Berty.Gameplay;
using Berty.UI.Card;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.UI.Card.Listeners
{
    public class HandCardInput : MonoBehaviour
    {
        private HandCardBehaviour behaviour;

        void Awake()
        {
            behaviour = GetComponent<HandCardBehaviour>();
        }

        public void CardClick()
        {
            if (behaviour.IsAnimating()) return;
            if (Input.GetMouseButtonDown(0)) HandleLeftClick();
        }

        public void CardFocusOn()
        {
            DisplayManager.Instance.ShowLookupCard(behaviour.Sprite);
        }

        public void CardFocusOff()
        {
            DisplayManager.Instance.HideLookupCard();
        }

        private void HandleLeftClick()
        {
            if (transform.parent.name.Contains("Dead")) behaviour.ReviveCard();
            else CardManager.Instance.ChangeSelection(behaviour);
        }
    }
}
