public class Tankbert : Character
{
    public Tankbert()
    {
        AddName("tankbert");
        AddProperties(Gender.Male, Role.Offensive);
        AddStats(5, 5, 2, 5);
        AddRange(0, 1, attackRange);
        AddRange(0, 2, attackRange);
        AddRange(0, 1, blockRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(0, -1, blockRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 1, riposteRange);
    }

    public override void SkillAdjustHealthChange(CardSprite card, int value)
    {
        if (0 <= value) return;
        card.AdvanceStrength(value);
        card.AdvanceDexterity(-value);
    }
}
