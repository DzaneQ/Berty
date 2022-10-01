using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class NewTransformState : CardState
{
    public NewTransformState(CardSprite sprite) : base(sprite)
    {

    }

    public override void HandleFieldCollision() { }

    public override void HandleClick()
    {
        card.ConfirmPayment();
    }

    public override void TakePaidAction() { }

    public override CardState AdjustTransformChange(int buttonIndex)
    {
        card.CancelPayment();
        return new ActiveState(card);
    }

    public override bool IsForPaymentConfirmation()
    {
        return true;
    }

    public override CardState SetActive => new ActiveState(card);
}
