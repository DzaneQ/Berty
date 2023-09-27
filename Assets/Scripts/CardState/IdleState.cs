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

    public override CardState AdjustTransformChange(int buttonIndex) => this;

    //public override bool IsJudgementRevenge()
    //{
    //    Debug.Log("Asking for judgement revenge in idle state.");
    //    return card.Grid.CurrentStatus.JudgementRevenge == card.OccupiedField.Align;
    //}

    public override CardState SetActive => new ActiveState(card);

    public override CardState SetTelecinetic => new TelecineticState(card);

    public override CardState SetTargetable => new TargetState(card);

    public override CardState SetIdle => this;
}
