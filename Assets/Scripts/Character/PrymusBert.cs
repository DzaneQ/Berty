public class PrymusBert : Character
{
    public PrymusBert()
    {
        AddName("prymus bert");
        AddProperties(Gender.Kid, Role.Support);
        AddStats(1, 3, 5, 3);
        AddRange(0, 1, attackRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, 0, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(0, -1, blockRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        AddRange(-1, 1, riposteRange);
    }
}
