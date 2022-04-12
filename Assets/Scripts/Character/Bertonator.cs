public class Bertonator : Character
{
    public Bertonator()
    {
        AddName("bertonator");
        AddProperties(Gender.Male, Role.Agile);
        AddStats(0, 3, 4, 4);
        AddRange(0, 1, attackRange);
        AddRange(0, 1, blockRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, 0, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        AddRange(-1, 1, riposteRange);
    }
}
