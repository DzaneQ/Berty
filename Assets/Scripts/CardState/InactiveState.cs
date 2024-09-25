using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class InactiveState : CardState
{
    public InactiveState(CardSprite sprite) : base(sprite)
    {
        if (card.OccupiedField == null) Debug.Log("Setting inactive state for " + card.name);
        else Debug.Log("Setting inactive state for " + card.name + " on field " + card.OccupiedField.name);
        card.gameObject.SetActive(false);
        card.DisableButtons();
        card.SetCardToDefaultTransform();
        card.UnhighlightCard();
    }

    public override CardState ActivateCard()
    {
        return new NewCardState(card);
    }
    public override void HandleClick()
    {
        card.LoadSelectedCard();
    }

    //public override CardState AdjustTransformChange(int buttonIndex)
    //{
    //    return this;
    //}

    //public override bool IsJudgementRevenge()
    //{
    //    return card.Grid.CurrentStatus.JudgementRevenge == card.Grid.Turn.CurrentAlignment;
    //}
}
