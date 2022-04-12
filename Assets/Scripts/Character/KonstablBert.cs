public class KonstablBert : Character
{
    public KonstablBert()
    {
        AddName("konstabl bert");
        AddProperties(Gender.Male, Role.Agile);
        AddStats(2, 3, 5, 3);
        AddRange(0, 2, attackRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(0, -1, blockRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, blockRange);
        AddRange(-1, 1, riposteRange);
    }
}
