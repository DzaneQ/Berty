public class BertPogromca : Character
{
    public BertPogromca()
    {
        AddName("bert pogromca");
        AddProperties(Gender.Male, Role.Offensive);
        AddStats(2, 3, 4, 4);
        AddRange(0, 2, attackRange);
        AddRange(-1, 1, attackRange);
        AddRange(0, 1, riposteRange);
        //AddRange(1, 1, riposteRange);
        AddRange(1, 0, riposteRange);
        //AddRange(1, -1, riposteRange);
        AddRange(0, -1, blockRange);
        //AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        //AddRange(-1, 1, riposteRange);
    }
}
