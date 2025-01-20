public class AstronautaBert : Character
{
    public AstronautaBert()
    {
        AddName("astronauta bert");
        AddProperties(Gender.Male, Role.Agile);
        AddStats(1, 2, 5, 2);
        AddRange(1, 1, attackRange);
        AddRange(1, -1, attackRange);
        AddRange(-1, -1, attackRange);
        AddRange(-1, 1, attackRange);
        AddRange(0, 1, blockRange);
        //AddRange(1, 1, riposteRange);
        AddRange(1, 0, blockRange);
        //AddRange(1, -1, riposteRange);
        AddRange(0, -1, blockRange);
        //AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, blockRange);
        //AddRange(-1, 1, riposteRange);
        AddSoundEffect("657936__matrixxx__satellite-signal-02");
    }

    public override void SkillAdjustPowerChange(int value, CardSprite card, CardSprite spellSource)
    {
        if (card.CardStatus.Power > 0) return;
        card.AdvancePower(Power, card);
        card.AdvanceHealth(-6);
    }
}
