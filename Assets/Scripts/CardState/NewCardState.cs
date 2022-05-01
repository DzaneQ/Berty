using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class NewCardState : CardState
{
    public override void InitiateState(CardSprite cardSprite)
    {
        card = cardSprite;
        card.gameObject.SetActive(true);
        card.ImportFromCardImage();
        card.UpdateHealthBar(card.Character.Health);
        card.UpdateRelativeCoordinates();
        card.CallPayment(card.Character.Power);
        card.ApplyPhysics();
    }

    public override void HandleFieldCollision()
    {
        card.ShowNeutralButtons();
        card.ApplyPhysics(false);
    }

    public override void HandleClick()
    {
        card.ConfirmPayment();
    }

    public override void TakePaidAction()
    {
        card.DefendNewStand();
    }

    public override CardState GoToNext()
    {
        return new ActiveState();
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
}
