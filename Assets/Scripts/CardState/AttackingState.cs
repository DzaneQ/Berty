using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class AttackingState : CardState
{
    public AttackingState(CardSprite sprite) : base(sprite)
    {
        card.EnableNeutralButton(1);
    }

    public override CardState DeactivateCard() => new InactiveState(card);

    public override void HandleClick()
    {
        Debug.Log("Attacking state go!");
        card.ConfirmPayment();
    }

    public override void TakePaidAction()
    {
        Debug.Log("Attack!");
        card.AttackWholeRange();
    }

    public override void Cancel()
    {
        card.CancelPayment();
    }

    public override bool IsForPaymentConfirmation()
    {
        return true;
    }
    public override CardState SetActive => new ActiveState(card);
}
