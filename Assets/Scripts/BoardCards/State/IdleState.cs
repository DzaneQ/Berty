using Berty.BoardCards.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.State
{
    /*internal class IdleState : CardState
    {
        public IdleState(BoardCardBehaviour sprite) : base(sprite)
        {
            //Debug.Log("Setting idle state for " + card.name + " on field " + card.OccupiedField.name);
            card.DisableButtons();
        }

        //public override CardState DeactivateCard() => new InactiveState(card);

        public override void HandleClick() { }

        public override CardState AdjustTransformChange(int buttonIndex) => this;

        public override bool Cancel()
        {
            return false;
        }

        public override CardState SetActive()
        {
            if (HasLostControl()) return this;
            return new ActiveState(card);
        }

        public override CardState SetTelecinetic()
        {
            if (HasLostControl()) return this;
            return new TelecineticState(card);
        }

        public override CardState SetTargetable()
        {
            if (HasLostControl()) return this;
            return new TargetState(card);
        }

        public override CardState SetIdle => this;

        public override void EnableButtons() { }

        private bool HasLostControl()
        {
            if (card.IsDead()) return true;
            if (card.HasLostWill()) return true;
            return false;
        }
    }*/
}