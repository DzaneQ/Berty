using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Managers;
using Berty.Enums;
using UnityEngine.SocialPlatforms.Impl;

namespace Berty.BoardCards.State
{
    public class AttackingState : CardState
    {
        public AttackingState(BoardCardStateMachine card) : base(card) { }

        protected override void ActivateButtonSet() { }

        public override void HandleLeftClick()
        {
            BoardCardActionManager.Instance.ConfirmPayment(stateMachine);
        }

        public override bool IsForPay() => true;

        public override bool IsDexterityBased() => throw new System.Exception("Attacking state should not check for dexterity buttons!");

        public override CardStateEnum GetNameEnum() => CardStateEnum.Attacking;
    }
}