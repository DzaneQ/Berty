using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class InactiveState : CardState
{
    public InactiveState(CardSprite sprite) : base(sprite)
    {
        //Debug.Log("Setting inactive state for " + card.name);
        card.gameObject.SetActive(false);
        card.DisableButtons();
        card.SetCardToDefaultTransform();
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
