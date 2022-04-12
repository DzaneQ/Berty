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
        card.PrepareToAttack();
    }

    public override CardState AdjustTransformChange(int buttonIndex)
    {
        card.CallPayment(6 - card.Character.Dexterity);
        card.EnableNeutralButton(buttonIndex);
        return new NewTransformState();
    }

    public override CardState GoToNext()
    {
        return new IdleState();
    }
}
