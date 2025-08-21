using Berty.UI.Card.Managers;
using Berty.Display.Managers;
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
            else HandCardSelectManager.Instance.ChangeSelection(behaviour);
        }
    }
}
