public class BertWick : Character
{
    public BertWick()
    {
        AddName("bert wick");
        AddProperties(Gender.Male, Role.Agile);
        AddStats(1, 3, 5, 2);
        AddRange(-1, 0, attackRange);
        AddRange(1, 1, attackRange);
        AddRange(0, 1, blockRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(0, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        AddRange(-1, 1, riposteRange);
    }

    public override void SkillAdjustHealthChange(int value, CardSprite card)
    {
        if (card.CardStatus.Health > 0 || card.CardStatus.Dexterity <= 0) return;
        card.AdvanceHealth(2);
        card.AdvanceStrength(1, card);
        card.AdvancePower(1, card);
        card.AdvanceDexterity(-1, card);
    }

    public override void SkillAdjustDexterityChange(int value, CardSprite card)
    {
        if (card.CardStatus.Dexterity == 0) card.AdvanceHealth(-6);
    }
}
