public class BertkaSerferka : Character
{
    public BertkaSerferka()
    {
        AddName("bertka serferka");
        AddProperties(Gender.Female, Role.Agile);
        AddStats(2, 3, 5, 4);
        AddRange(0, 2, attackRange);
        AddRange(1, 2, attackRange);
        AddRange(-1, 2, attackRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, 0, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(0, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        AddRange(-1, 1, riposteRange);
    }
}
