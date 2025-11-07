using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Button;
using Berty.BoardCards.Managers;
using Berty.Enums;
using UnityEngine;

namespace Berty.BoardCards.State
{
    public class NewTransformState : CardState
    {
        private NavigationEnum navigation;

        public NewTransformState(BoardCardStateMachine card, NavigationEnum navigatingFrom) : base(card)
        {
            navigation = navigatingFrom;
        }

        protected override void ActivateButtonSet()
        {
            stateMachine.GetButton(GetNavigationOppositeTo(navigation)).ActivateNeutralButton();
        }

        public override void HandleLeftClick()
        {
            BoardCardActionManager.Instance.ConfirmPayment(stateMachine);
        }

        public override bool IsForPay() => true;

        public override bool IsOnNewMove()
        {
            return navigation == NavigationEnum.MoveUp
                || navigation == NavigationEnum.MoveRight
                || navigation == NavigationEnum.MoveDown
                || navigation == NavigationEnum.MoveLeft;
        }

        public override bool IsDexterityBased() => false;

        public override CardStateEnum GetNameEnum() => CardStateEnum.NewTransform;

        private NavigationEnum GetNavigationOppositeTo(NavigationEnum navigation)
        {
            return navigation switch
            {
                NavigationEnum.MoveUp => NavigationEnum.MoveDown,
                NavigationEnum.MoveRight => NavigationEnum.MoveLeft,
                NavigationEnum.MoveDown => NavigationEnum.MoveUp,
                NavigationEnum.MoveLeft => NavigationEnum.MoveRight,
                NavigationEnum.RotateLeft => NavigationEnum.RotateRight,
                NavigationEnum.RotateRight => NavigationEnum.RotateLeft,
                _ => throw new System.Exception("Unknown opposite direction"),
            };
        }
    }
}