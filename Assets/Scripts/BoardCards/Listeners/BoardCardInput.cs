using Berty.BoardCards;
using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Button;
using Berty.BoardCards.Managers;
using Berty.Display.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Card.Managers;
using System;
using UnityEngine;

namespace Berty.BoardCards.Listeners
{
    public class BoardCardInput : MonoBehaviour
    {
        private BoardCardCore behaviour;
        private Game game;
        private Camera cam;

        void Awake()
        {
            behaviour = GetComponent<BoardCardCore>();
            game = CoreManager.Instance.Game;
            cam = Camera.main;
        }

        void Start()
        {
            // Enabling toggle from the inspector.
        }

        void OnEnable()
        {
            if (!IsCursorOnCard()) return;
            behaviour.CardNavigation.EnableButtons();
            EventManager.Instance.RaiseOnHighlightStart(behaviour);

        }

        void OnDisable()
        {
            behaviour.CardNavigation.DisableButtons();
        }

        public void OnMouseOver()
        {
            if (IsLeftClicked()) HandleLeftClick();
            else if (IsRightClicked()) HandleSideClick();
        }

        public void OnMouseEnter()
        {
            DisplayManager.Instance.ShowLookupCard(behaviour.Sprite);
            if (behaviour.CardNavigation.IsCardAnimating()) return;
            EventManager.Instance.RaiseOnHighlightStart(behaviour);
        }

        public void OnMouseExit()
        {
            if (IsCursorOnCard()) return;
            DisplayManager.Instance.HideLookupCard();
            EventManager.Instance.RaiseOnHighlightEnd();
        }

        private bool IsCursorOnCard() // Check if cursor is on the card or card's button
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit)) return false;
            if (hit.collider == null) return false;
            if (hit.transform == transform) return true; // Is cursor on card square object?
            if (hit.transform.parent == null) return false;
            return hit.transform.parent.parent == transform; // Is cursor on card's button object?
        }

        private bool IsLeftClicked()
        {
            return Input.GetMouseButtonDown(0);
        }

        private bool IsRightClicked()
        {
            return Input.GetMouseButtonDown(1);
        }

        private void HandleLeftClick()
        {
            if (!IsLeftClicked()) return;
            if (TryPuttingAnExtraCard()) return;
            switch (behaviour.CardState)
            {
                case CardStateEnum.Active:
                    BoardCardActionManager.Instance.PrepareToAttack(behaviour);
                    break;
                case CardStateEnum.NewTransform:
                case CardStateEnum.NewCard:
                case CardStateEnum.Attacking:
                    BoardCardActionManager.Instance.ConfirmPayment(behaviour);
                    break;
                case CardStateEnum.Idle:
                case CardStateEnum.Telekinetic:
                    break;
                case CardStateEnum.Effectable:
                    BoardCardActionManager.Instance.ApplySpecialEffect(behaviour);
                    break;
                default:
                    throw new Exception("Clicked on card of an unknown state");
            }
        }

        private void HandleSideClick()
        {
            switch (behaviour.BoardCard.GetSkill())
            {
                case SkillEnum.GotkaBerta:
                    if (game.CardPile.AreThereAnyDeadCards())
                    {
                        StatusManager.Instance.AddUniqueStatusWithAlignment(StatusEnum.RevivalSelect, behaviour.BoardCard.Align);
                        behaviour.RemoveCard();
                    }
                    break;
            }
        }

        private bool TryPuttingAnExtraCard()
        {
            if (SelectionManager.Instance.IsItPaymentTime()) return false;
            if (!HasSelectedOneCard()) return false;
            if (behaviour.BoardCard.GetSkill() != SkillEnum.TrenerPokebertow) return false;
            if (behaviour.BoardCard.Align != game.CurrentAlignment) return false;
            behaviour.ParentField.SendMessage("PutTheCard");
            return true;
        }

        private bool HasSelectedOneCard()
        {
            return SelectionManager.Instance.GetTheOnlySelectedCardOrNull() != null;
        }
    }
}
