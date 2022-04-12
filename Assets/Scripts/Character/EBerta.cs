public class EBerta : Character
{
    public EBerta()
    {
        AddName("eberta");
        AddProperties(Gender.Female, Role.Offensive);
        AddStats(1, 2, 5, 3);
        AddRange(0, -1, attackRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, 0, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(0, -1, blockRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        AddRange(-1, 1, riposteRange);
    }
}
