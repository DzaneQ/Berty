using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class TelecineticState : CardState
{
    public TelecineticState(CardSpriteBehaviour sprite) : base(sprite)
    {
        card.PrepareDexterityButtons();
        EnableButtons();
    }

    public override CardState DeactivateCard() => new InactiveState(card);

    public override void HandleClick() { }

    public override CardState AdjustTransformChange(int buttonIndex)
    {
        if (card.OccupiedField.IsAligned(card.Grid.Turn.CurrentAlignment))
            throw new System.Exception("Trying to adjust transform on telecinetic owned card!");
        int dexterity = card.Grid.CurrentStatus.TelekinesisDex;
        return new NewTransformState(card, buttonIndex, dexterity);
    }

    public override CardState SetActive() => new ActiveState(card);

    public override CardState SetIdle => new IdleState(card);

    public override void EnableButtons()
    {
        //card.ShowDexterityButtons(true);
        card.ShowButtons(true, false);
    }
}
