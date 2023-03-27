public class BertVentura : Character
{
    public BertVentura()
    {
        AddName("bert ventura");
        AddProperties(Gender.Male, Role.Offensive);
        AddStats(0, 2, 4, 5);
        AddRange(0, 1, attackRange);
        AddRange(1, 0, attackRange);
        AddRange(-1, 0, attackRange);
        AddRange(0, 1, riposteRange);
        //AddRange(1, 1, riposteRange);
        AddRange(1, 0, riposteRange);
        //AddRange(1, -1, riposteRange);
        AddRange(0, -1, riposteRange);
        //AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        //AddRange(-1, 1, riposteRange);
    }
}
