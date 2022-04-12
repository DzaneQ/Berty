public class KuglarzBert : Character
{
    public KuglarzBert()
    {
        AddName("kuglarz bert");
        AddProperties(Gender.Male, Role.Agile);
        AddStats(1, 2, 6, 2);
        AddRange(1, 1, attackRange);
        AddRange(-1, 2, attackRange);
        AddRange(0, 1, riposteRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, 0, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 1, riposteRange);
    }
}
