using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class InactiveState : CardState
{
    public InactiveState(CardSprite sprite) : base(sprite)
    {
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
        card.TryToActivateCard();
    }

    //public override bool IsJudgementRevenge()
    //{
    //    return card.Grid.CurrentStatus.JudgementRevenge == card.Grid.Turn.CurrentAlignment;
    //}
}
