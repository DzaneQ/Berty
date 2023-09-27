using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class NewCardState : CardState
{
    public NewCardState(CardSprite sprite) : base(sprite)
    {
        card.ActivateCard();
    }

    public override CardState DeactivateCard() => new InactiveState(card);

    public override void HandleFieldCollision()
    {
        if (!card.OccupiedField.AreThereTwoCards()) card.ShowNeutralButtons();
        else card.EnableNeutralButton(1);
        card.ApplyPhysics(false);
    }

    public override void HandleClick()
    {
        card.ConfirmPayment();
    }

    public override CardState TakePaidAction()
    {
        card.ConfirmNewCard();
        card.ResetAttack();
        return new IdleState(card);
    }

    public override CardState AdjustTransformChange(int buttonIndex)
    {
        return this;
    }

    public override void Cancel()
    {
        card.CancelCard();
        card.CancelPayment();
    }

    public override bool IsForPaymentConfirmation()
    {
        return true;
    }

    public override CardState SetActive => new ActiveState(card);
}
