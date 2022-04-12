public class BertWick : Character
{
    public BertWick()
    {
        AddName("bert wick");
        AddProperties(Gender.Male, Role.Agile);
        AddStats(1, 3, 5, 2);
        AddRange(-1, 0, attackRange);
        AddRange(1, 1, attackRange);
        AddRange(0, 1, blockRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(0, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        AddRange(-1, 1, riposteRange);
    }
}
