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
        card.ConfirmPayment(true);
    }

    public override void TakePaidAction()
    {
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
