﻿public class BertaGejsza : Character
{
    public BertaGejsza()
    {
        AddName("berta gejsza");
        AddProperties(Gender.Female, Role.Support);
        AddStats(1, 2, 5, 3);
        AddRange(-1, 1, attackRange);
        AddRange(0, 1, riposteRange);
        //AddRange(1, 1, riposteRange);
        AddRange(1, 0, blockRange);
        //AddRange(1, -1, riposteRange);
        //AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        //AddRange(-1, 1, riposteRange);
        AddSoundEffect("217656__reitanna__knuckles-cracking");
    }

    public override void SkillOnNewCard(CardSprite card)
    {
        foreach (CardSprite adjCard in card.GetAdjacentCards()) SkillOnNeighbor(card, adjCard);
    }

    public override void SkillOnNeighbor(CardSprite card, CardSprite target)
    {
        if (card.IsAllied(target.OccupiedField)) target.AdvanceDexterity(-1, card);
        else target.AdvanceDexterity(-3, card);
        target.AddResistance(this);
    }

    public override void SkillOnMove(CardSprite card) => SkillOnNewCard(card);
}