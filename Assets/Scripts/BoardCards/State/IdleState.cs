using Berty.BoardCards.Behaviours;
using Berty.Enums;

namespace Berty.BoardCards.State
{
    public class IdleState : CardState
    {
        public IdleState(BoardCardStateMachine card) : base(card) { }

        protected override void ActivateButtonSet() { }

        public override void HandleLeftClick() { }

        public override bool IsForPay() => false;

        public override bool IsDexterityBased() => throw new System.Exception("Idle state should not check for dexterity buttons!");

        public override CardStateEnum GetNameEnum() => CardStateEnum.Idle;
    }
}