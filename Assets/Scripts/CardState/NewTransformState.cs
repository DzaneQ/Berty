using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class NewTransformState : CardState
{
    public override void InitiateState(CardSprite cardSprite)
    {
        card = cardSprite;
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
        return GoToNext();
    }

    public override CardState GoToNext()
    {
        return new ActiveState();
    }

    public override bool IsForPaymentConfirmation()
    {
        return true;
    }
}
