using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class NewCardState : CardState
{
    public NewCardState(CardSprite sprite) : base(sprite)
    {
        Debug.Log("Setting new card state for " + card.name + " on field " + card.OccupiedField.name);
        card.ActivateNewCard();
        EnableButtons();
    }

    public override CardState DeactivateCard() => new InactiveState(card);

    public override void HandleClick()
    {
        card.ConfirmPayment();
    }

    public override CardState TakePaidAction()
    {
        card.ConfirmNewCard();
        card.ResetAttack();
        return new IdleState(card);
    }

    public override CardState AdjustTransformChange(int buttonIndex)
    {
        return this;
    }

    public override void Cancel()
    {
        card.CancelCard();
        card.CancelPayment();
    }

    public override bool IsForPaymentConfirmation()
    {
        return true;
    }

    public override CardState SetActive => new ActiveState(card);

    public override void EnableButtons()
    {
        if (!card.OccupiedField.AreThereTwoCards()) card.ShowNeutralButtons();
        else card.EnableCancelNeutralButton(1);
    }
}
