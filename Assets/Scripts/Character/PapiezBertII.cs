public class PapiezBertII : Character
{
    public PapiezBertII()
    {
        AddName("papiez bert II");
        AddProperties(Gender.Male, Role.Special);
        AddStats(2, 3, 5, 3);
        AddRange(0, 2, attackRange);
        AddRange(0, 1, blockRange);
        AddRange(1, 1, riposteRange);
        AddRange(1, 0, riposteRange);
        AddRange(1, -1, riposteRange);
        AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        AddRange(-1, 1, riposteRange);
    }
}
