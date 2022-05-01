using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class AttackingState : CardState
{
    public override void InitiateState(CardSprite cardSprite)
    {
        card = cardSprite;
        card.EnableNeutralButton(1);
    }

    public override void HandleClick()
    {
        Debug.Log("Attacking state go!");
        card.ConfirmPayment(true);
    }

    public override void TakePaidAction()
    {
        Debug.Log("Attack!");
        card.Attack();
    }

    public override CardState GoToNext()
    {
        return new ActiveState();
    }

    public override void Cancel()
    {
        card.CancelPayment();
    }

    public override bool IsForPaymentConfirmation()
    {
        return true;
    }
}
