public class BertkaIdolka : Character
{
    public BertkaIdolka()
    {
        AddName("bertka idolka");
        AddProperties(Gender.Female, Role.Support);
        AddStats(2, 2, 4, 4);
        AddRange(0, 1, attackRange);
        AddRange(1, 1, attackRange);
        AddRange(-1, 1, attackRange);
        AddRange(0, 1, blockRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(0, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 1, riposteRange);
    }
}
