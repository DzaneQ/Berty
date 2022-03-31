using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class ActiveState : CardState
{
    public override void InitiateState(CardSprite cardSprite)
    {
        card = cardSprite;
        card.ShowDexterityButtons();
    }

    public override void HandleFieldCollision() { }

    public override void HandleClick()
    {
        Debug.Log("Attack!"); // TODO: Attacking mechanic.
    }

    public override CardState HandleAlignmentChange()
    {
        return GoToNext();
    }

    public override CardState AdjustTransformChange(int buttonIndex)
    {
        card.CallPayment();
        card.EnableNeutralButton(buttonIndex);
        return new NewTransformState();
    }

    public override CardState GoToNext()
    {
        return new IdleState();
    }
}
