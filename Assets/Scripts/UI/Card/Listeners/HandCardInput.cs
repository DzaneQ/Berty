using Berty.UI.Card.Managers;
using Berty.Display.Managers;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Utility;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Berty.UI.Card.Listeners
{
    public class HandCardInput : MonoBehaviour
    {
        private HandCardBehaviour behaviour;
        private Game game;

        private void Awake()
        {
            behaviour = GetComponent<HandCardBehaviour>();
            game = EntityLoadManager.Instance.Game;
        }

        public void CardClick()
        {
            if (ManagerLocator.TurnManagerInstance.IsItNotMyTurn()) return;
            if (behaviour.IsAnimating()) return;
            if (Mouse.current.leftButton.wasPressedThisFrame) HandleLeftClick();
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
            if (transform.parent.name.Contains("Dead")) ManagerLocator.ApplyManualEffectManagerInstance.ReviveCard(behaviour);
            else HandCardSelectManager.Instance.ChangeSelection(behaviour);
        }
    }
}
