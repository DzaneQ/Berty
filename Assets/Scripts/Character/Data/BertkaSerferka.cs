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
        AddSoundEffect("756350__indythecringemaster__retro-ocean-noise");
    }

    public override bool SkillSpecialAttack(CardSpriteBehaviour card)
    {
        //bool swap = true;
        //Field[] target = new Field[AttackRange.Count];
        CardSpriteBehaviour[] targetCard = new CardSpriteBehaviour[AttackRange.Count];
        Field swapTarget = null;
        for (int i = 0; i < AttackRange.Count; i++)
        {
            int index = (i + 2) % AttackRange.Count;
            Field targetField = card.GetTargetField(AttackRange[index]);
            if (targetField == null || !targetField.IsOpposed(card.OccupiedField.Align)) continue;
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
            targetCard[index] = targetField.OccupantCard;
            swapTarget = targetField;
        }
        if (swapTarget == null) return true;
        for (int i = 0; i < AttackRange.Count; i++)
        {
            if (targetCard[i] == null) continue;
            if (targetCard[i] != swapTarget.OccupantCard) targetCard[i].TakeDamage(card.GetStrength(), card.OccupiedField);
            else
            {
                card.SwapWith(swapTarget);
                targetCard[i].TakeDamage(card.GetStrength(), card.OccupiedField);
                break;
            }
        }
        /*Field srcField = card.OccupiedField;
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
        if (swapTarget != null) card.SwapWith(swapTarget);*/
        return true;
    }
}
