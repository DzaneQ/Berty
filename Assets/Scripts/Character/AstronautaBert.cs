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
        AddRange(1, 1, riposteRange);
        AddRange(1, 0, blockRange);
        AddRange(1, -1, riposteRange);
        AddRange(0, -1, blockRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, blockRange);
        AddRange(-1, 1, riposteRange);
    }
}
