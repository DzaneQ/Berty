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
        //AddRange(1, 1, riposteRange);
        AddRange(1, 0, riposteRange);
        //AddRange(1, -1, riposteRange);
        AddRange(0, -1, riposteRange);
        //AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        //AddRange(-1, 1, riposteRange);
    }

    // TODO: Fix bug that makes this character not rotate when killing target.

    public override bool SkillSpecialAttack(CardSprite card)
    {
        //bool swap = true;
        Field srcField = card.OccupiedField;
        Field swapTarget = null;
        for (int i = 0; i < AttackRange.Count; i++)
        {
            int[] distance = AttackRange[(i + 2) % AttackRange.Count];
            Field targetField = card.GetTargetField(distance);
            if (targetField == null || !targetField.IsOccupied() || card.IsAllied(targetField)) continue;
            bool advantage = false;
            switch (i)
            {
                case 0:
                    advantage = targetField.OccupantCard.GetStrength() <= card.GetStrength();
                    break;
                case 1:
                    advantage = targetField.OccupantCard.CardStatus.Power <= card.CardStatus.Power;
                    break;
                case 2:
                    advantage = targetField.OccupantCard.CardStatus.Dexterity <= card.CardStatus.Dexterity;
                    break;
            }
            if (!advantage) continue;
            //if (swap)
            //{
            //    card.SwapWith(targetField);
            //    swap = false;
            //    targetField = srcField;
            //}
            swapTarget = targetField;
            targetField.OccupantCard.TakeDamage(card.GetStrength(), srcField);
        }
        if (swapTarget != null) card.SwapWith(swapTarget);
        return true;
    }
}
