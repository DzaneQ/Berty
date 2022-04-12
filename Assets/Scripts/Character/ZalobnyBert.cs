public class ZalobnyBert : Character
{
    public ZalobnyBert()
    {
        AddName("zalobny bert");
        AddProperties(Gender.Male, Role.Special);
        AddStats(0, 3, 3, 4);
        AddRange(0, 1, attackRange);
        AddRange(0, 1, riposteRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(0, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        AddRange(-1, 1, riposteRange);
    }
}
