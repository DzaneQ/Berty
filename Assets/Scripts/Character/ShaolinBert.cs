public class ShaolinBert : Character
{
    public ShaolinBert()
    {
        AddName("shaolin bert");
        AddProperties(Gender.Male, Role.Support);
        AddStats(1, 3, 5, 4);
        AddRange(1, 1, attackRange);
        AddRange(-1, 1, attackRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(0, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 1, riposteRange);
    }

    public override void SkillOnNewCard(CardSprite card)
    {
        foreach (Field field in card.Grid.Fields)
            if (field.IsOccupied() && !card.IsAllied(field)) 
                field.OccupantCard.AdvanceStrength(-field.OccupantCard.CardStatus.Power / 3, card);
    }
}
