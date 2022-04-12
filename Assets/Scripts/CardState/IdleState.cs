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

    public override void HandleFieldCollision() { }

    public override void HandleClick() { }

    public override CardState GoToNext()
    {
        return new ActiveState();
    }
}
