using Berty.UI.Card.Managers;
using Berty.Display.Managers;
using UnityEngine;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;

namespace Berty.UI.Card.Listeners
{
    public class HandCardInput : MonoBehaviour
    {
        private HandCardBehaviour behaviour;
        private Game game;

        void Awake()
        {
            behaviour = GetComponent<HandCardBehaviour>();
            game = EntityLoadManager.Instance.Game;
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
            if (transform.parent.name.Contains("Dead")) HandCardActionManager.Instance.ReviveCard(behaviour);
            else HandCardSelectManager.Instance.ChangeSelection(behaviour);
        }
    }
}
