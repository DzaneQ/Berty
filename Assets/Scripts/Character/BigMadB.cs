public class BigMadB : Character
{
    public BigMadB()
    {
        AddName("big mad b");
        AddProperties(Gender.Male, Role.Special);
        AddStats(1, 3, 5, 4);
        AddRange(1, 0, attackRange);
        AddRange(-1, 0, attackRange);
        //AddRange(1, 1, riposteRange);
        AddRange(1, 0, blockRange);
        //AddRange(1, -1, riposteRange);
        //AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, blockRange);
        //AddRange(-1, 1, riposteRange);
    }
}
