public class SamurajBert : Character
{
    public SamurajBert()
    {
        AddName("samuraj bert");
        AddProperties(Gender.Male, Role.Offensive);
        AddStats(2, 3, 3, 4);
        AddRange(0, 1, attackRange);
        AddRange(1, 0, attackRange);
        AddRange(-1, 0, attackRange);
        AddRange(0, 1, blockRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, 0, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        AddRange(-1, 1, riposteRange);
    }
}
