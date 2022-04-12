public class KoszmarZBertwood : Character
{
    public KoszmarZBertwood()
    {
        AddName("koszmar z bertwood");
        AddProperties(Gender.Kid, Role.Special);
        AddStats(3, 5, 3, 5);
        AddRange(0, 1, attackRange);
        AddRange(1, 0, attackRange);
        AddRange(1, -1, attackRange);
        AddRange(0, -1, attackRange);
        AddRange(-1, -1, attackRange);
        AddRange(-1, 0, attackRange);
        AddRange(-1, 1, attackRange);
        AddRange(0, 1, blockRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, 0, blockRange);
        AddRange(1, -1, riposteRange);
        AddRange(0, -1, blockRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 1, riposteRange);
    }
}
