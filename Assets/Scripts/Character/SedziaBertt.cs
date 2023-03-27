public class SedziaBertt : Character
{
    public SedziaBertt()
    {
        AddName("sedzia bertt");
        AddProperties(Gender.Male, Role.Special);
        AddStats(1, 4, 4, 4);
        AddRange(1, 2, attackRange);
        AddRange(1, 1, attackRange);
        AddRange(-1, 1, attackRange);
        AddRange(-1, 2, attackRange);
        AddRange(0, 1, riposteRange);
        //AddRange(1, 1, riposteRange);
        AddRange(1, 0, blockRange);
        //AddRange(1, -1, riposteRange);
        AddRange(0, -1, blockRange);
        //AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        //AddRange(-1, 1, riposteRange);
    }

    public override void SkillOnNewCard(CardSprite card)
    {
        card.Grid.SetJudgement();
    }

    public override void SkillOnDeath(CardSprite card)
    {
        card.Grid.RemoveJudgement(card.OccupiedField.Align);
    }
}
