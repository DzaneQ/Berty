public class GotkaBerta : Character
{
    public GotkaBerta()
    {
        AddName("gotka berta");
        AddProperties(Gender.Female, Role.Support);
        AddStats(1, 4, 5, 4);
        AddRange(0, 1, attackRange);
        AddRange(0, 1, riposteRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 1, riposteRange);
    }
}
