using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class NewTransformState : CardState
{
    private int displayedButtonIndex;

    //public NewTransformState(CardSprite sprite) : base(sprite)
    //{

    //}

    public NewTransformState(CardSprite sprite, int buttonIndex) : base(sprite)
    {
        displayedButtonIndex = buttonIndex;
        card.EnableNeutralButton(buttonIndex);
    }

    public override void HandleFieldCollision() { }

    public override void HandleClick()
    {
        card.ConfirmPayment();
    }

    public override CardState TakePaidAction()
    {
        if (IsMoving()) card.ConfirmMove();
        return new IdleState(card);
    }

    public override CardState AdjustTransformChange(int buttonIndex)
    {
        card.CancelPayment();
        return new ActiveState(card);
    }

    public override bool IsForPaymentConfirmation()
    {
        return true;
    }

    public override CardState SetActive => new ActiveState(card);

    private bool IsMoving()
    {
        if (4 <= displayedButtonIndex && displayedButtonIndex <= 7) return true;
        return false;
    }
}
