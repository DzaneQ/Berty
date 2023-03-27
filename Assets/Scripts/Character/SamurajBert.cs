public class SamurajBert : Character
{
    private bool activeSpell = true;
    public SamurajBert()
    {
        AddName("samuraj bert");
        AddProperties(Gender.Male, Role.Offensive);
        AddStats(2, 3, 3, 4);
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
    }

    public override void SkillOnNewCard(CardSprite card)
    {
        activeSpell = true;
        SkillOnNeighbor(card, card);
    }

    public override void SkillOnNeighbor(CardSprite card, CardSprite target)
    {
        if (!activeSpell || card.GetAdjacentCards().Count < 3) return;
        card.AdvanceDexterity(1, card);
        card.AdvancePower(1, card);
        activeSpell = false;
    }

    public override void SkillOnMove(CardSprite card) => SkillOnNeighbor(card, card);
}
