using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Button;
using Berty.Enums;
using UnityEngine;

namespace Berty.BoardCards.State
{
    public abstract class CardState
    {
        protected BoardCardStateMachine stateMachine;

        public CardState(BoardCardStateMachine card)
        {
            stateMachine = card;
        }

        public void OnStateEnter()
        {
            DeactivateAllButtons();
            ActivateButtonSet();
        }

        public abstract void HandleLeftClick();
        public abstract bool IsForPay();
        public virtual bool IsOnNewMove() => false;
        public abstract bool IsDexterityBased();
        public abstract CardStateEnum GetNameEnum();
        protected abstract void ActivateButtonSet();
        protected void DeactivateAllButtons()
        {
            foreach (CardButton button in stateMachine.Buttons) button.DeactivateButton();
        }
    }
}