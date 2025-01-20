public class SedziaBertt : Character
{
    public SedziaBertt()
    {
        AddName("sedzia bertt");
        AddProperties(Gender.Male, Role.Special);
        AddStats(1, 4, 4, 4);
        AddRange(1, 2, attackRange);
        AddRange(1, 1, attackRange);
        AddRange(-1, 1, attackRange);
        AddRange(-1, 2, attackRange);
        AddRange(0, 1, riposteRange);
        //AddRange(1, 1, riposteRange);
        AddRange(1, 0, blockRange);
        //AddRange(1, -1, riposteRange);
        AddRange(0, -1, blockRange);
        //AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        //AddRange(-1, 1, riposteRange);
        AddSoundEffect("351429__kinoton__gun-laser-single-shot-sci-fi");
    }

    public override void SkillOnNewCard(CardSprite card)
    {
        card.Grid.SetJudgement(card.OccupiedField.Align);
    }

    public override void SkillOnDeath(CardSprite card)
    {
        card.Grid.RemoveJudgement();
    }

    public override void SkillOnOtherCardDeath(CardSprite card, CardSprite source)
    {
        if (card.Grid.CurrentStatus.IsJudgement) return;
        if (!source.OccupiedField.IsOpposed(card.OccupiedField.Align)) return;
        card.Grid.SetJudgement(card.OccupiedField.Align);
    }

    public override void SkillAdjustPowerChange(int value, CardSprite card, CardSprite spellSource)
    {
        if (card.CardStatus.Power > 0) return;
        if (card.OccupiedField.Align == Alignment.Opponent) card.Grid.SetJudgement(Alignment.Player);
        else if (card.OccupiedField.Align == Alignment.Opponent) card.Grid.SetJudgement(Alignment.Opponent);
        else throw new System.Exception("Unidentified align for judgement change");
    }
}
