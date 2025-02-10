using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class ActiveState : CardState
{
    public ActiveState(CardSprite sprite) : base(sprite)
    {
        //Debug.Log("Setting active state for " + card.name + " on field " + card.OccupiedField.name);
        card.PrepareDexterityButtons();
        EnableButtons();
        //card.ShowDexterityButtons();
    }

    public override void HandleClick() // Experimental change.
    {
        if (!card.IsCardSelected()) card.PrepareToAttack();
        else card.Character.SkillCardClick(card);
    }

    public override void HandleSideClick()
    {
        if (card.CanUseSkill()) card.Character.SkillSideClick(card);
    }

    public override CardState AdjustTransformChange(int buttonIndex)
    {
        if (!card.OccupiedField.IsAligned(card.Grid.Turn.CurrentAlignment)) 
            throw new System.Exception("Trying to adjust transform on active non-owned card!");
        int dexterity = card.CardStatus.Dexterity;
        if (buttonIndex > 3 && card.CanBeTelecinetic() && card.Grid.CurrentStatus.TelekinesisDex > dexterity)
            dexterity = card.Grid.CurrentStatus.TelekinesisDex;
        return new NewTransformState(card, buttonIndex, dexterity);
    }

    //public override bool IsJudgementRevenge()
    //{
    //    Debug.Log("Asking for judgement revenge in active state.");
    //    return card.Grid.CurrentStatus.JudgementRevenge == card.OccupiedField.Align;
    //}

    public override CardState SetTargetable() => new TargetState(card);

    public override CardState SetIdle => new IdleState(card);

    public override void EnableButtons()
    {
        //card.ShowDexterityButtons();
        card.ShowButtons(true, true);
    }
}
