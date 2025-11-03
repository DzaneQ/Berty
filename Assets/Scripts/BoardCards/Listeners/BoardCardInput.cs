using Berty.BoardCards.Behaviours;
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
    public class BoardCardInput : BoardCardBehaviour
    {
        private Game game;
        private Camera cam;

        protected override void Awake()
        {
            base.Awake();
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
            Core.Navigation.EnableButtons();
            EventManager.Instance.RaiseOnHighlightStart(Core);

        }

        void OnDisable()
        {
            Core.Navigation.DisableButtons();
        }

        public void OnMouseOver()
        {
            if (IsLeftClicked()) HandleLeftClick();
            else if (IsRightClicked()) HandleSideClick();
        }

        public void OnMouseEnter()
        {
            DisplayManager.Instance.ShowLookupCard(Core.Sprite);
            if (Core.Navigation.IsCardAnimating()) return;
            EventManager.Instance.RaiseOnHighlightStart(Core);
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
            switch (Core.CardState)
            {
                case CardStateEnum.Active:
                    BoardCardActionManager.Instance.PrepareToAttack(Core);
                    break;
                case CardStateEnum.NewTransform:
                case CardStateEnum.NewCard:
                case CardStateEnum.Attacking:
                    BoardCardActionManager.Instance.ConfirmPayment(Core);
                    break;
                case CardStateEnum.Idle:
                case CardStateEnum.Telekinetic:
                    break;
                case CardStateEnum.Effectable:
                    BoardCardActionManager.Instance.ApplySpecialEffect(Core);
                    break;
                default:
                    throw new Exception("Clicked on card of an unknown state");
            }
        }

        private void HandleSideClick()
        {
            switch (Core.BoardCard.GetSkill())
            {
                case SkillEnum.GotkaBerta:
                    if (game.CardPile.AreThereAnyDeadCards())
                    {
                        StatusManager.Instance.AddUniqueStatusWithAlignment(StatusEnum.RevivalSelect, Core.BoardCard.Align);
                        Core.RemoveCard();
                    }
                    break;
            }
        }

        private bool TryPuttingAnExtraCard()
        {
            if (SelectionManager.Instance.IsItPaymentTime()) return false;
            if (!HasSelectedOneCard()) return false;
            if (Core.BoardCard.GetSkill() != SkillEnum.TrenerPokebertow) return false;
            if (Core.BoardCard.Align != game.CurrentAlignment) return false;
            Core.ParentField.SendMessage("PutTheCard");
            return true;
        }

        private bool HasSelectedOneCard()
        {
            return SelectionManager.Instance.GetTheOnlySelectedCardOrNull() != null;
        }
    }
}
