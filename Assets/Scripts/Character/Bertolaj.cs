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
        //AddRange(1, 1, riposteRange);
        //AddRange(1, -1, riposteRange);
        AddRange(0, -1, riposteRange);
        //AddRange(-1, -1, riposteRange);
        //AddRange(-1, 1, riposteRange);
        AddSoundEffect("441649__crownieyt__open-package-box-parcel");
    }

    public override bool SkillSpecialAttack(CardSprite card)
    {
        foreach (int[] distance in card.Character.AttackRange)
        {
            Field targetField = card.GetTargetField(distance);
            if (targetField == null || !targetField.IsOccupied()) continue;
            if (targetField.OccupantCard.CardStatus.Power > 3) continue;
            targetField.OccupantCard.TakeDamage(card.GetStrength(), card.OccupiedField);
        }
        return true;
    }

    public override void SkillAdjustPowerChange(int value, CardSprite card, CardSprite source)
    {
        if (card.CardStatus.Power <= 0) card.Grid.AddCardIntoQueue(source.OccupiedField.Align);
    }
}