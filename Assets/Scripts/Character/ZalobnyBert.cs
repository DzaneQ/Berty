using System.Collections.Generic;

public class ZalobnyBert : Character
{
    public ZalobnyBert()
    {
        AddName("zalobny bert");
        AddProperties(Gender.Male, Role.Special);
        AddStats(0, 3, 3, 4);
        AddRange(0, 1, attackRange);
        AddRange(0, 1, riposteRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(0, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        AddRange(-1, 1, riposteRange);
    }

    public override int SkillDefenceModifier(int damage, CardSprite attacker)
    {
        if (attacker.GetRole() == Role.Offensive) return 0;
        return damage;
    }

    public override void SkillAdjustHealthChange(int value, CardSprite card)
    {
        if (0 <= value) return;
        foreach (CardSprite adjCard in card.GetAdjacentCards())
            if (card.IsAllied(adjCard.OccupiedField))
                adjCard.AdvanceHealth(-2 * value);
    }
}
