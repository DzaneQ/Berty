public class KonstablBert : Character
{
    public KonstablBert()
    {
        AddName("konstabl bert");
        AddProperties(Gender.Male, Role.Agile);
        AddStats(2, 3, 5, 3);
        AddRange(0, 2, attackRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(0, -1, blockRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, blockRange);
        AddRange(-1, 1, riposteRange);
    }

    public override void SkillOnNewCard(CardSprite card)
    {
        foreach (Field field in card.Grid.Fields)
        {
            if (!field.IsOccupied()) continue;
            if (field.OccupantCard.GetRole() == Role.Special) field.OccupantCard.AdvanceHealth(-1);
        }
    }

    public override bool SkillSpecialAttack(CardSprite card)
    {
        foreach (int[] distance in AttackRange)
        {
            Field targetField = card.GetTargetField(distance);
            if (targetField == null || !targetField.IsOccupied()) continue;
            switch (targetField.OccupantCard.GetRole())
            {
                case Role.Special:
                case Role.Support:
                    targetField.OccupantCard.TakeDamage(card.CardStatus.Strength + 1, card.OccupiedField);
                    break;
                default:
                    targetField.OccupantCard.TakeDamage(card.CardStatus.Strength, card.OccupiedField);
                    break;
            }
            UnityEngine.Debug.Log("Attack - X: " + targetField.GetX() + "; Y: " + targetField.GetY());
        }
        return true;
    }
}
