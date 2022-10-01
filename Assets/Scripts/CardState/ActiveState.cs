using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class ActiveState : CardState
{
    public ActiveState(CardSprite sprite) : base(sprite)
    {
        card.ShowDexterityButtons();
    }

    public override void HandleFieldCollision()
    {
        card.ApplyPhysics(false);
    }

    public override void HandleClick()
    {
        card.PrepareToAttack();
    }

    public override CardState AdjustTransformChange(int buttonIndex)
    {
        card.CallPayment(6 - card.Character.Dexterity);
        card.EnableNeutralButton(buttonIndex);
        return new NewTransformState(card);
    }

    public override CardState SetIdle => new IdleState(card);
}
