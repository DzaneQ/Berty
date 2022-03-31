using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class IdleState : CardState
{
    public override void InitiateState(CardSprite cardSprite)
    {
        card = cardSprite;
        card.DisableButtons();
    }

    public override void HandleFieldCollision()
    {
        throw new System.Exception("Card in idle state colliding.");
    }

    public override void HandleClick() { }

    public override CardState HandleAlignmentChange()
    {
        return GoToNext();
    }

    public override CardState AdjustTransformChange(int buttonIndex)
    {
        throw new System.Exception("Idle card shouldn't change transform.");
    }

    public override CardState GoToNext()
    {
        return new ActiveState();
    }
}
