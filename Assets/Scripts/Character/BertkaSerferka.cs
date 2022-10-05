public class BertkaSerferka : Character
{
    public BertkaSerferka()
    {
        AddName("bertka serferka");
        AddProperties(Gender.Female, Role.Agile);
        AddStats(2, 3, 5, 4);
        AddRange(0, 2, attackRange);
        AddRange(1, 2, attackRange);
        AddRange(-1, 2, attackRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, 0, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(0, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        AddRange(-1, 1, riposteRange);
    }

    public override bool SkillSpecialAttack(CardSprite card)
    {
        bool swap = true;
        Field srcField = card.OccupiedField;
        for (int i = AttackRange.Count - 1; i > 0; i--)
        {
            int[] distance = AttackRange[(i + 2) % AttackRange.Count];
            Field targetField = card.GetTargetField(distance);
            if (targetField == null || !targetField.IsOccupied() || card.IsAllied(targetField)) continue;
            bool advantage = false;
            switch (i)
            {
                case 0:
                    advantage = targetField.OccupantCard.CardStatus.Strength <= card.CardStatus.Strength;
                    break;
                case 1:
                    advantage = targetField.OccupantCard.CardStatus.Power <= card.CardStatus.Power;
                    break;
                case 2:
                    advantage = targetField.OccupantCard.CardStatus.Dexterity <= card.CardStatus.Dexterity;
                    break;
            }
            if (!advantage) continue;
            if (swap)
            {
                card.SwapWith(targetField);
                swap = false;
                targetField = srcField;
            }
            targetField.OccupantCard.TakeDamage(card.CardStatus.Strength, srcField);
        }
        return true;
    }

    //public override bool SkillSpecialAttack(CardSprite card)
    //{
    //    CardSprite[] attackedCards = new CardSprite[AttackRange.Count];
    //    bool swap = true;
    //    for (int i = 0; i < AttackRange.Count; i++)
    //    {
    //        int[] distance = AttackRange[i];
    //        Field targetField = card.GetTargetField(distance);
    //        if (targetField == null || !targetField.IsOccupied() || card.IsAllied(targetField)) continue;
    //        bool advantage = false;
    //        switch (i)
    //        {
    //            case 2:
    //                advantage = targetField.OccupantCard.CardStatus.Strength <= card.CardStatus.Strength;
    //                break;
    //            case 0:
    //                advantage = targetField.OccupantCard.CardStatus.Power <= card.CardStatus.Power;
    //                break;
    //            case 1:
    //                advantage = targetField.OccupantCard.CardStatus.Dexterity <= card.CardStatus.Dexterity;
    //                break;
    //        }
    //        if (!advantage) continue;
    //        attackedCards[i + 1 % AttackRange.Count] = targetField.OccupantCard;
    //    }
    //    for (int i = AttackRange.Count - 1; i > 0; i--)
    //    {
    //        if (attackedCards[i] == null) continue;
    //        if (swap) Swap
    //        targetField.OccupantCard.TakeDamage(card.CardStatus.Strength, card.OccupiedField);
    //    }
    //    if (swapTarget == null) return true;
    //    card.SwapWith(swapTarget);
    //    return true;
    //}
}
