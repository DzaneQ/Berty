public class BertaGejsza : Character
{
    public BertaGejsza()
    {
        AddName("berta gejsza");
        AddProperties(Gender.Female, Role.Support);
        AddStats(1, 2, 5, 3);
        AddRange(-1, 1, attackRange);
        AddRange(0, 1, riposteRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, 0, blockRange);
        AddRange(1, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        AddRange(-1, 1, riposteRange);
    }

    public override void SkillOnNewCard(CardSprite card)
    {
        foreach (CardSprite adjCard in card.GetAdjacentCards()) SkillOnNeighbor(card, adjCard);
    }

    public override void SkillOnNeighbor(CardSprite card, CardSprite target)
    {
        if (card.IsAllied(target.OccupiedField)) target.AdvanceDexterity(-1);
        else target.AdvanceDexterity(-3);
    }
}