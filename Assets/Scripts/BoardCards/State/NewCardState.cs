using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Button;
using Berty.BoardCards.Managers;
using Berty.Enums;
using UnityEngine;

namespace Berty.BoardCards.State
{
    public class NewCardState : CardState
    {
        public NewCardState(BoardCardStateMachine card) : base(card) { }

        protected override void ActivateButtonSet()
        {
            if (!IsTheOnlyCardOnBoard()) return;
            stateMachine.GetButton(NavigationEnum.RotateLeft).ActivateNeutralButton();
            stateMachine.GetButton(NavigationEnum.RotateRight).ActivateNeutralButton();
        }

        public override void HandleLeftClick()
        {
            BoardCardActionManager.Instance.ConfirmPayment(stateMachine);
        }

        public override bool IsForPay() => true;

        public override bool IsDexterityBased() => false;

        public override CardStateEnum GetNameEnum() => CardStateEnum.NewCard;

        private bool IsTheOnlyCardOnBoard()
        {
            Transform cardSet = stateMachine.transform.parent;
            return cardSet.childCount <= 1;
        }
    }
}