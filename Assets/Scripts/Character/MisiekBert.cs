public class MisiekBert : Character
{
    public MisiekBert()
    {
        AddName("misiek bert");
        AddProperties(Gender.Male, Role.Special);
        AddStats(1, 3, 4, 4);
        AddRange(0, 1, attackRange);
        AddRange(1, 0, attackRange);
        AddRange(-1, 0, attackRange);
        AddRange(0, 1, blockRange);
        //AddRange(1, 1, riposteRange);
        AddRange(1, 0, riposteRange);
        //AddRange(1, -1, riposteRange);
        //AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        //AddRange(-1, 1, riposteRange);
        AddSoundEffect("661402__sascharettberg__dj_puzzle_scratch_02-0-1.2");
    }

    public override void SkillOnNewCard(CardSprite card)
    {
        foreach (CardSprite adjCard in card.GetAdjacentCards()) adjCard.RotateCard(270);  
    }

    public override void SkillOnAttack(CardSprite card) => SkillOnNewCard(card);

    public override void SkillOnMove(CardSprite card) => SkillOnNewCard(card);
}
