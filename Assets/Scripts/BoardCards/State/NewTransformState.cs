using Berty.BoardCards.Animation;
using Berty.BoardCards.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.State
{
    /*internal class NewTransformState : CardState
    {
        private int displayedButtonIndex;

        public NewTransformState(BoardCardBehaviour sprite, int buttonIndex, int dexterity) : base(sprite)
        {
            //Debug.Log("Setting new transform state for " + card.name + " on field " + card.OccupiedField.name);
            card.CallPayment(6 - dexterity);
            displayedButtonIndex = buttonIndex;
            EnableButtons();
            //card.EnableCancelNeutralButton(buttonIndex);
        }

        //public override void HandleFieldCollision() { }

        public override void HandleClick()
        {
            card.ConfirmPayment();
        }

        public override CardState TakePaidAction(AnimatingCardSprite animating)
        {
            if (animating != null) animating.ConfirmSound();
            if (IsMoving()) card.ConfirmMove();
            return new IdleState(card);
        }

        public override CardState AdjustTransformChange(int buttonIndex) // TEST! Potential bug fiesta!
        {
            card.CancelPayment();
            //if (card.OccupiedField.IsAligned(card.Grid.Turn.CurrentAlignment)) return new ActiveState(card);
            //else return new TelecineticState(card);
            return card.ResultState;
        }

        public override bool IsForPaymentConfirmation()
        {
            return true;
        }

        public override CardState SetActive() => new ActiveState(card);

        public override CardState SetTelecinetic() => new TelecineticState(card);

        private bool IsMoving()
        {
            if (4 <= displayedButtonIndex && displayedButtonIndex <= 7) return true;
            return false;
        }

        public override void EnableButtons()
        {
            card.EnableCancelNeutralButton(displayedButtonIndex);
        }
    }*/
}