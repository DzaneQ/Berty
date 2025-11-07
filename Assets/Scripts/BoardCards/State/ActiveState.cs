using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Button;
using Berty.BoardCards.Managers;
using Berty.Enums;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Berty.BoardCards.State
{
    public class ActiveState : CardState
    {
        public ActiveState(BoardCardStateMachine card) : base(card) { }

        protected override void ActivateButtonSet()
        {
            stateMachine.GetButton(NavigationEnum.RotateLeft).ActivateDexterityButton();
            stateMachine.GetButton(NavigationEnum.RotateRight).ActivateDexterityButton();
            stateMachine.GetButton(NavigationEnum.MoveUp).ActivateDexterityButton();
            stateMachine.GetButton(NavigationEnum.MoveRight).ActivateDexterityButton();
            stateMachine.GetButton(NavigationEnum.MoveDown).ActivateDexterityButton();
            stateMachine.GetButton(NavigationEnum.MoveLeft).ActivateDexterityButton();
        }

        public override void HandleLeftClick()
        {
            BoardCardActionManager.Instance.PrepareToAttack(stateMachine);
        }

        public override bool IsForPay() => false;

        public override bool IsDexterityBased() => true;

        public override CardStateEnum GetNameEnum() => CardStateEnum.Active;
    }
}