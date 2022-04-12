public class KowbojBert : Character
{
    public KowbojBert()
    {
        AddName("kowboj bert");
        AddProperties(Gender.Male, Role.Agile);
        AddStats(2, 3, 4, 3);
        AddRange(1, 1, attackRange);
        AddRange(-1, 1, attackRange);
        AddRange(0, 1, riposteRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, 0, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        AddRange(-1, 1, riposteRange);
    }
}
