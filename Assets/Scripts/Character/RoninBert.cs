﻿public class RoninBert : Character
{
    public RoninBert()
    {
        AddName("ronin bert");
        AddProperties(Gender.Male, Role.Offensive);
        AddStats(2, 3, 4, 3);
        AddRange(0, 1, attackRange);
        AddRange(0, 2, attackRange);
        AddRange(0, 1, blockRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, 0, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        AddRange(-1, 1, riposteRange);
    }

    public override bool SkillSpecialAttack(CardSprite card)
    {
        Field[] target = new Field[AttackRange.Count];
        Field lastTarget = null;
        int targetPower = 0;
        for (int i = AttackRange.Count - 1; 0 <= i; i--)
        {
            target[i] = card.GetTargetField(AttackRange[i]);
            if (target[i] == null || !target[i].IsOccupied()) continue;
            lastTarget = target[i];
            break;
        }
        if (lastTarget == null) return true;
        for (int i = 0; i < AttackRange.Count; i++)
        {
            if (target[i] == null || !target[i].IsOccupied()) continue;
            if (target[i].OccupantCard.CardStatus.Power > targetPower) targetPower = target[i].OccupantCard.CardStatus.Power;
            if (target[i] != lastTarget) target[i].OccupantCard.TakeDamage(card.CardStatus.Strength, card.OccupiedField);
            else
            {
                CardSprite targetCard = lastTarget.OccupantCard;
                card.SwapWith(lastTarget);
                targetCard.TakeDamage(card.CardStatus.Strength, card.OccupiedField);
                break;
            }
        }
        if (targetPower > card.CardStatus.Power) card.AdvanceHealth(-1);
        return true;
    }
}
