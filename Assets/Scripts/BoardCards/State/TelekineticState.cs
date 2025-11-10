using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Button;
using Berty.Enums;
using UnityEngine;

namespace Berty.BoardCards.State
{
    public class TelekineticState : CardState
    {
        public TelekineticState(BoardCardStateMachine card) : base(card) { }

        protected override void ActivateButtonSet()
        {
            stateMachine.GetButton(NavigationEnum.MoveUp).TryActivatingDexterityButton();
            stateMachine.GetButton(NavigationEnum.MoveRight).TryActivatingDexterityButton();
            stateMachine.GetButton(NavigationEnum.MoveDown).TryActivatingDexterityButton();
            stateMachine.GetButton(NavigationEnum.MoveLeft).TryActivatingDexterityButton();
        }

        public override void HandleLeftClick() { }

        public override bool IsForPay() => false;

        public override bool IsDexterityBased() => true;

        public override CardStateEnum GetNameEnum() => CardStateEnum.Telekinetic;
    }
}