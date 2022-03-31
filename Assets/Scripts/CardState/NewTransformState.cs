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

    public override CardState HandleAlignmentChange()
    {
        throw new System.Exception("Alignment change shouldn't occur in new transform state.");
    }

    public override CardState AdjustTransformChange(int buttonIndex)
    {
        card.CancelPayment();
        return GoToNext();
    }

    public override CardState GoToNext()
    {
        return new ActiveState();
    }

}
