using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class NewCardState : CardState
{
    public override void InitiateState(CardSprite cardSprite)
    {
        card = cardSprite;
        card.gameObject.SetActive(true);
        card.CallPayment();
        card.ImportFromImage();
        card.ApplyPhysics();
    }

    public override void HandleFieldCollision()
    {
        card.ShowNeutralButtons();
    }

    public override void HandleClick()
    {
        card.ConfirmPayment();
    }

    public override CardState HandleAlignmentChange()
    {
        throw new System.Exception("Alignment change shouldn't occur in new card state.");
    }

    public override CardState AdjustTransformChange(int buttonIndex)
    {
        throw new System.Exception("Transform change shouldn't have any effect on free card.");
    }
    
    public override CardState GoToNext()
    {
        return new ActiveState();
    }

}
