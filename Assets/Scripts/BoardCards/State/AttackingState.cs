using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Managers;
using Berty.Enums;
using UnityEngine;

namespace Berty.BoardCards.State
{
    public class AttackingState : CardState
    {
        public AttackingState(BoardCardStateMachine card) : base(card) { }

        protected override void ActivateButtonSet()
        {
            Debug.Log("Executing button set activationg for attacking state.");
        }

        public override void HandleLeftClick()
        {
            Debug.Log("Handling left click for attacking state");
            BoardCardActionManager.Instance.ConfirmPayment(stateMachine);
        }

        public override bool IsForPay() => true;

        public override bool IsDexterityBased() => throw new System.Exception("Attacking state should not check for dexterity buttons!");

        public override CardStateEnum GetNameEnum() => CardStateEnum.Attacking;
    }
}