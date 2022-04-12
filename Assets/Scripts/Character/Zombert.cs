public class Zombert : Character
{
    public Zombert()
    {
        AddName("zombert");
        AddProperties(Gender.Male, Role.Offensive);
        AddStats(1, 2, 4, 3);
        AddRange(0, 1, attackRange);
        AddRange(0, 1, riposteRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, 0, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(0, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        AddRange(-1, 1, riposteRange);
    }
}
