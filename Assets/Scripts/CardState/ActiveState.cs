using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class ActiveState : CardState
{
    public ActiveState(CardSprite sprite) : base(sprite)
    {
        Debug.Log("Setting active state for " + card.name);
        card.ShowDexterityButtons();
    }

    public override void HandleFieldCollision()
    {
        card.ApplyPhysics(false);
    }

    public override void HandleClick()
    {
        if (!card.IsCardSelected()) card.PrepareToAttack();
        else if (!card.Character.SkillCardClick(card)) card.CardManager.DeselectCards();
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
        if (buttonIndex > 3 && card.Grid.IsTelekineticMovement() && card.Grid.CurrentStatus.TelekinesisDex > dexterity)
            dexterity = card.Grid.CurrentStatus.TelekinesisDex;
        card.CallPayment(6 - dexterity);
        return new NewTransformState(card, buttonIndex);
    }

    //public override bool IsJudgementRevenge()
    //{
    //    Debug.Log("Asking for judgement revenge in active state.");
    //    return card.Grid.CurrentStatus.JudgementRevenge == card.OccupiedField.Align;
    //}

    public override CardState SetIdle => new IdleState(card);
}
