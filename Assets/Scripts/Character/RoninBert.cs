public class RoninBert : Character
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
}
