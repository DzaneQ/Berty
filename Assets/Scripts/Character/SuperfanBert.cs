public class SuperfanBert : Character
{
    public SuperfanBert()
    {
        AddName("superfan bert");
        AddProperties(Gender.Kid, Role.Agile);
        AddStats(1, 2, 5, 2);
        AddRange(1, 2, attackRange);
        AddRange(-1, 2, attackRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, 0, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        AddRange(-1, 1, riposteRange);
    }
}
