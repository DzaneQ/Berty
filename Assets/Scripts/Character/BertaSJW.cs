public class BertaSJW : Character
{
    public BertaSJW()
    {
        AddName("berta sjw");
        AddProperties(Gender.Female, Role.Special);
        AddStats(1, 3, 5, 3);
        AddRange(-1, 0, attackRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, 0, blockRange);
        AddRange(1, -1, riposteRange);
        AddRange(0, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        AddRange(-1, 1, riposteRange);
    }
}
