public class BertaSJW : Character
{
    public BertaSJW()
    {
        AddName("berta sjw");
        AddProperties(Gender.Female, Role.Special);
        AddStats(1, 3, 5, 3);
        AddRange(-1, 0, attackRange);
        //AddRange(1, 1, riposteRange);
        AddRange(1, 0, blockRange);
        //AddRange(1, -1, riposteRange);
        AddRange(0, -1, riposteRange);
        //AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        //AddRange(-1, 1, riposteRange);
        AddSoundEffect("418417__wormer2__water-splash-emptying-a-bucket-9");
    }

    public override void SkillOnNewCard(CardSpriteBehaviour card)
    {
        foreach (CardSpriteBehaviour adjCard in card.GetAdjacentCards()) SkillOnNeighbor(card, adjCard);
    }

    public override void SkillOnNeighbor(CardSpriteBehaviour card, CardSpriteBehaviour target)
    {
        target.AdvancePower(-3, card);
        target.AddResistance(this);
    }

    public override void SkillOnMove(CardSpriteBehaviour card) => SkillOnNewCard(card);
}
