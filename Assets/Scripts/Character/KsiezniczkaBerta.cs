public class KsiezniczkaBerta : Character
{
    public KsiezniczkaBerta()
    {
        AddName("ksiezniczka berta");
        AddProperties(Gender.Female, Role.Support);
        AddStats(1, 4, 4, 6);
        AddRange(1, 1, attackRange);
        AddRange(0, 1, blockRange);
        //AddRange(1, 1, riposteRange);
        AddRange(1, 0, riposteRange);
        //AddRange(1, -1, riposteRange);
        //AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        //AddRange(-1, 1, riposteRange);
    }
}
