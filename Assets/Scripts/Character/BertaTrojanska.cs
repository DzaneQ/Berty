public class BertaTrojanska : Character
{
    public BertaTrojanska()
    {
        AddName("berta trojanska");
        AddProperties(Gender.Female, Role.Support);
        AddStats(1, 2, 5, 3);
        AddRange(0, 2, attackRange);
        AddRange(0, 1, riposteRange);
        //AddRange(1, 1, riposteRange);
        AddRange(1, 0, blockRange);
        //AddRange(1, -1, riposteRange);
        //AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, blockRange);
        //AddRange(-1, 1, riposteRange);
    }

    public override void SkillOnNewCard(CardSprite card)
    {
        foreach (CardSprite adjCard in card.GetAdjacentCards()) SkillOnNeighbor(card, adjCard);
    }

    public override void SkillOnNeighbor(CardSprite card, CardSprite target)
    {
        if (!card.IsAllied(target.OccupiedField)) target.AdvanceStrength(-1, card);
        else target.AdvancePower(1, card);
        target.AddResistance(this);
    }

    public override void SkillOnMove(CardSprite card) => SkillOnNewCard(card);
}
