public class Bertolaj : Character
{
    public Bertolaj()
    {
        AddName("bertolaj");
        AddProperties(Gender.Male, Role.Special);
        AddStats(1, 4, 4, 3);
        AddRange(0, 1, attackRange);
        AddRange(1, 1, attackRange);
        AddRange(1, 0, attackRange);
        AddRange(1, -1, attackRange);
        AddRange(0, -1, attackRange);
        AddRange(-1, -1, attackRange);
        AddRange(-1, 0, attackRange);
        AddRange(-1, 1, attackRange);
        AddRange(0, 1, riposteRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(0, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 1, riposteRange);
    }
}