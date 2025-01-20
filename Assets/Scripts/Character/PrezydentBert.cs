﻿public class PrezydentBert : Character
{
    public PrezydentBert()
    {
        AddName("prezydent bert");
        AddProperties(Gender.Male, Role.Support);
        AddStats(1, 3, 5, 4);
        AddRange(0, 1, attackRange);
        AddRange(0, -1, attackRange);
        AddRange(0, 1, riposteRange);
        //AddRange(1, 1, riposteRange);
        //AddRange(1, -1, riposteRange);
        //AddRange(-1, -1, riposteRange);
        //AddRange(-1, 1, riposteRange);
        AddSoundEffect("557114__firediesproductions__fdp-coin-flip-3");
    }

    public override void SkillOnAttack(CardSprite card)
    {
        foreach (CardSprite adjCard in card.GetAdjacentCards()) if (card.IsAllied(adjCard.OccupiedField)) adjCard.AdvanceStrength(1, card);
        card.AdvancePower(-1, card);
    }

    public override void SkillOnMove(CardSprite card)
    {
        SkillOnAttack(card);
    }
}
