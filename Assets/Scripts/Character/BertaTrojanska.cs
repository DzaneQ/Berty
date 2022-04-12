public class BertaTrojanska : Character
{
    public BertaTrojanska()
    {
        AddName("berta trojanska");
        AddProperties(Gender.Female, Role.Support);
        AddStats(1, 2, 5, 3);
        AddRange(0, 2, attackRange);
        AddRange(0, 1, riposteRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, 0, blockRange);
        AddRange(1, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, blockRange);
        AddRange(-1, 1, riposteRange);
    }
}
