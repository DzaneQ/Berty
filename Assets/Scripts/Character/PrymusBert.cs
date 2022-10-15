public class PrymusBert : Character
{
    public PrymusBert()
    {
        AddName("prymus bert");
        AddProperties(Gender.Kid, Role.Support);
        AddStats(1, 3, 5, 3);
        AddRange(0, 1, attackRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, 0, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(0, -1, blockRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        AddRange(-1, 1, riposteRange);
    }

    public override int SkillDefenceModifier(int damage, CardSprite attacker)
    {
        if (damage > 0) return damage - 1;
        return damage;
    }

    public override void SkillOnNewCard(CardSprite card)
    {
        foreach (CardSprite adjCard in card.GetAdjacentCards()) SkillOnNeighbor(card, adjCard);
    }

    public override void SkillOnNeighbor(CardSprite card, CardSprite target)
    {
        target.AdvancePower(3);
    }
}
