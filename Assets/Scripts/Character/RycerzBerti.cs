public class RycerzBerti : Character
{
    public RycerzBerti()
    {
        AddName("rycerz berti");
        AddProperties(Gender.Female, Role.Support);
        AddStats(2, 4, 5, 4);
        AddRange(0, 2, attackRange);
        AddRange(1, 0, attackRange);
        AddRange(0, 1, riposteRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, 0, blockRange);
        AddRange(1, -1, riposteRange);
        AddRange(0, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 1, riposteRange);
    }
}
