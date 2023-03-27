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
        card.CallPayment(6 - card.CardStatus.Dexterity);
        return new NewTransformState(card, buttonIndex);
    }

    //public override bool IsJudgementRevenge()
    //{
    //    Debug.Log("Asking for judgement revenge in active state.");
    //    return card.Grid.CurrentStatus.JudgementRevenge == card.OccupiedField.Align;
    //}

    public override CardState SetIdle => new IdleState(card);
}
