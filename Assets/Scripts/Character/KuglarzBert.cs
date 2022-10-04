public class KuglarzBert : Character
{
    public KuglarzBert()
    {
        AddName("kuglarz bert");
        AddProperties(Gender.Male, Role.Agile);
        AddStats(1, 2, 6, 2);
        AddRange(1, 1, attackRange);
        AddRange(-1, 2, attackRange);
        AddRange(0, 1, riposteRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, 0, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 1, riposteRange);
    }

    public override void SkillOnSuccessfulAttack(CardSprite card)
    {
        card.AdvanceDexterity(-1);
        card.AdvanceHealth(1);      
    }

    public override void SkillOnNewCard(CardSprite card)
    {
        for (int i = 0; i < 4; i++)
        {
            Field targetField = card.GetAdjacentField(i * 90);
            if (targetField == null || !card.IsAllied(targetField)) continue;
            targetField.OccupantCard.AdvancePower(1);
        }    
    }
}
