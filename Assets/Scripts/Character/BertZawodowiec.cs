public class BertZawodowiec : Character
{
    public BertZawodowiec()
    {
        AddName("bert zawodowiec");
        AddProperties(Gender.Male, Role.Agile);
        AddStats(2, 4, 3, 4);
        AddRange(1, 1, attackRange);
        AddRange(2, 2, attackRange);
        AddRange(0, 1, riposteRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, 0, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(0, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        AddRange(-1, 1, riposteRange);
    }
}
