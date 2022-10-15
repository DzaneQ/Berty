﻿public class BertaAmazonka : Character
{
    public BertaAmazonka()
    {
        AddName("berta amazonka");
        AddProperties(Gender.Female, Role.Agile);
        AddStats(2, 3, 4, 3);
        AddRange(0, 1, attackRange);
        AddRange(0, 2, attackRange);
        AddRange(0, 1, riposteRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(0, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 1, riposteRange);
    }

    public override bool SkillSpecialAttack(CardSprite card)
    {
        foreach (int[] distance in card.Character.AttackRange)
        {
            Field targetField = card.GetTargetField(distance);
            if (targetField == null || !targetField.IsOccupied()) continue;
            targetField.OccupantCard.TakeDamage(card.CardStatus.Strength, card.OccupiedField);
            int[] neighbor = distance.Clone() as int[];
            neighbor[0]--;
            targetField = card.GetTargetField(neighbor);
            if (targetField != null && targetField.IsOccupied()) targetField.OccupantCard.AdvanceHealth(-1);
            neighbor[0] = neighbor[0] + 2;
            targetField = card.GetTargetField(neighbor);
            if (targetField != null && targetField.IsOccupied()) targetField.OccupantCard.AdvanceHealth(-1);
        }
        return true;
    }
}
