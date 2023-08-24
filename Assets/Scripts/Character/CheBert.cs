public class CheBert : Character
{
    public CheBert()
    {
        AddName("che bert");
        AddProperties(Gender.Male, Role.Special);
        AddStats(1, 3, 5, 3);
        AddRange(0, 1, attackRange);
        AddRange(0, 1, riposteRange);
        //AddRange(1, 1, riposteRange);
        //AddRange(1, -1, riposteRange);
        AddRange(0, -1, riposteRange);
        //AddRange(-1, -1, riposteRange);
        //AddRange(-1, 1, riposteRange);
    }
}
