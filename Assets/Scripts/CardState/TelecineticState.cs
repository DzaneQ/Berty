using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class TelecineticState : CardState
{
    public TelecineticState(CardSprite sprite) : base(sprite)
    {
        card.ShowDexterityButtons(true);
    }

    public override void HandleClick() { }

    public override CardState AdjustTransformChange(int buttonIndex)
    {
        if (card.OccupiedField.IsAligned(card.Grid.Turn.CurrentAlignment))
            throw new System.Exception("Trying to adjust transform on telecinetic owned card!");
        int dexterity = card.Grid.CurrentStatus.TelekinesisDex;
        card.CallPayment(6 - dexterity);
        return new NewTransformState(card, buttonIndex);
    }

    public override CardState SetActive => new ActiveState(card);

    public override CardState SetIdle => new IdleState(card);
}
