using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Button;
using Berty.BoardCards.Managers;
using Berty.Enums;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Berty.BoardCards.State
{
    public class EffectableState : CardState
    {
        public EffectableState(BoardCardStateMachine card) : base(card) { }

        protected override void ActivateButtonSet() { }

        public override void HandleLeftClick()
        {
            BoardCardActionManager.Instance.ApplySpecialEffect(stateMachine);
        }

        public override bool IsForPay() => false;

        public override bool IsDexterityBased() => throw new System.Exception("Effectable state should not check for dexterity buttons!");

        public override CardStateEnum GetNameEnum() => CardStateEnum.Effectable;
    }
}