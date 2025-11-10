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
            stateMachine.GetButton(NavigationEnum.RotateLeft).TryActivatingDexterityButton();
            stateMachine.GetButton(NavigationEnum.RotateRight).TryActivatingDexterityButton();
            stateMachine.GetButton(NavigationEnum.MoveUp).TryActivatingDexterityButton();
            stateMachine.GetButton(NavigationEnum.MoveRight).TryActivatingDexterityButton();
            stateMachine.GetButton(NavigationEnum.MoveDown).TryActivatingDexterityButton();
            stateMachine.GetButton(NavigationEnum.MoveLeft).TryActivatingDexterityButton();
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