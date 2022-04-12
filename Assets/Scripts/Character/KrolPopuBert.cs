public class KrolPopuBert : Character
{
    public KrolPopuBert()
    {
        AddName("krol popu bert");
        AddProperties(Gender.Male, Role.Special);
        AddStats(1, 3, 5, 2);
        AddRange(0, 1, attackRange);
        AddRange(1, 2, attackRange);
        AddRange(-1, 2, attackRange);
        AddRange(0, 1, riposteRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, 0, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        AddRange(-1, 1, riposteRange);
    }
}
