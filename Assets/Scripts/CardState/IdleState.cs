using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class IdleState : CardState
{
    public IdleState(CardSprite sprite) : base(sprite)
    {
        card.DisableButtons();
    }

    public override CardState DeactivateCard() => new InactiveState(card);

    public override void HandleFieldCollision()
    {
        card.ApplyPhysics(false);
    }

    public override void HandleClick() { }

    public override CardState SetActive => new ActiveState(card);

    public override CardState SetIdle => this;
}
