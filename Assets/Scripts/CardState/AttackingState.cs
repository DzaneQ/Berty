using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class AttackingState : CardState
{
    public AttackingState(CardSprite sprite) : base(sprite)
    {
        //Debug.Log("Setting attacking state for " + card.name + " on field " + card.OccupiedField.name);
        EnableButtons();
    }

    public override CardState DeactivateCard() => new InactiveState(card);

    public override void HandleClick()
    {
        //Debug.Log("Attacking state go!");
        card.ConfirmPayment();
    }

    public override CardState TakePaidAction(AnimatingCardSprite animating)
    {
        Debug.Log("Attack!");
        if (animating != null) animating.AttackingSound(card.Character);
        card.OrderAttack();
        return new IdleState(card);
    }

    public override CardState AdjustTransformChange(int buttonIndex)
    {
        //Debug.Log($"{card} makes a swap!");
        return this;
    }

    public override bool Cancel()
    {
        card.CancelPayment();
        return true;
    }

    public override bool IsForPaymentConfirmation() => true;

    //public override bool IsJudgementRevenge()
    //{
    //    Debug.Log("Asking for judgement revenge in attacking state.");
    //    return card.Grid.CurrentStatus.JudgementRevenge == card.OccupiedField.Align;
    //}

    public override CardState SetActive => new ActiveState(card);

    public override void EnableButtons()
    {
        card.EnableCancelNeutralButton(1);
    }
}
