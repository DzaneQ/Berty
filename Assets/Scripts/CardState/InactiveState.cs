using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class InactiveState : CardState
{
    public override void InitiateState(CardSprite cardSprite)
    {
        card = cardSprite;
        card.gameObject.SetActive(false);
        card.DisableButtons();
        card.SetCardToDefaultTransform();
        card.ResetAttack();
    }

    public override void HandleClick()
    {
        throw new System.Exception("You shouldn't be able to click this.");
    }

    public override CardState AdjustTransformChange(int buttonIndex)
    {
        throw new System.Exception("Inactive card shouldn't change transform.");
    }

    public override CardState GoToNext()
    {
        return new NewCardState();
    }
}
