public class KrzyzowiecBert : Character
{
    public KrzyzowiecBert()
    {
        AddName("krzyzowiec bert");
        AddProperties(Gender.Male, Role.Offensive);
        AddStats(2, 3, 5, 4);
        AddRange(0, 1, attackRange);
        AddRange(0, 1, riposteRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, 0, blockRange);
        AddRange(1, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, blockRange);
        AddRange(-1, 1, riposteRange);
    }
}
