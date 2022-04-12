public class BertaAmazonka : Character
{
    public BertaAmazonka()
    {
        AddName("berta amazonka");
        AddProperties(Gender.Female, Role.Agile);
        AddStats(2, 3, 4, 3);
        AddRange(0, 1, attackRange);
        AddRange(0, 2, attackRange);
        AddRange(0, 1, riposteRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(0, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 1, riposteRange);
    }
}
