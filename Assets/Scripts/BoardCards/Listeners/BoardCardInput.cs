using Berty.BoardCards.Behaviours;
using Berty.Display.Managers;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.UI.Card.Managers;
using UnityEngine;

namespace Berty.BoardCards.Listeners
{
    public class BoardCardInput : BoardCardBehaviour
    {
        private Camera cam;

        protected override void Awake()
        {
            base.Awake();
            cam = Camera.main;
        }

        void Start()
        {
            // Enabling toggle from the inspector.
        }

        public void OnMouseOver()
        {
            if (IsLeftClicked()) HandleLeftClick();
            else if (IsRightClicked()) HandleRightClick();
        }

        public void OnMouseEnter()
        {
            DisplayManager.Instance.ShowLookupCard(Sprite.LookupSprite);
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
            return Core.IsCursorFocused();
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
            StateMachine.HandleLeftClick();
        }

        private void HandleRightClick()
        {
            if (BoardCard.Align != game.CurrentAlignment) return;
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
            Core.ParentField.PutTheCard();
            return true;
        }

        private bool HasSelectedOneCard()
        {
            return SelectionManager.Instance.GetTheOnlySelectedCardOrNull() != null;
        }
    }
}
