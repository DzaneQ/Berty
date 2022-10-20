public class KowbojBert : Character
{
    public KowbojBert()
    {
        AddName("kowboj bert");
        AddProperties(Gender.Male, Role.Agile);
        AddStats(2, 3, 4, 3);
        AddRange(1, 1, attackRange);
        AddRange(-1, 1, attackRange);
        AddRange(0, 1, riposteRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, 0, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        AddRange(-1, 1, riposteRange);
    }

    public override void SkillOnSuccessfulAttack(CardSprite card)
    {
        card.AdvanceDexterity(1, card);
        foreach (CardSprite adjCard in card.GetAdjacentCards()) 
            if (card.IsAllied(adjCard.OccupiedField)) 
                adjCard.AdvanceDexterity(1, card);
    }
}
