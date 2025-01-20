public class KoszmarZBertwood : Character
{
    private bool attackPause;

    public KoszmarZBertwood()
    {
        AddName("koszmar z bertwood");
        AddProperties(Gender.Kid, Role.Special);
        AddStats(3, 5, 3, 5);
        AddRange(0, 1, attackRange);
        AddRange(1, 0, attackRange);
        AddRange(1, -1, attackRange);
        AddRange(0, -1, attackRange);
        AddRange(-1, -1, attackRange);
        AddRange(-1, 0, attackRange);
        AddRange(-1, 1, attackRange);
        AddRange(0, 1, blockRange);
        //AddRange(1, 1, riposteRange);
        AddRange(1, 0, blockRange);
        //AddRange(1, -1, riposteRange);
        AddRange(0, -1, blockRange);
        //AddRange(-1, -1, riposteRange);
        //AddRange(-1, 1, riposteRange);
        AddSoundEffect("564485__rizzard__monster-growl");
    }

    public override void SkillOnNewCard(CardSprite card)
    {
        attackPause = false;
    }

    public override void SkillOnAttack(CardSprite card)
    {
        attackPause = true;
    }

    public override bool SkillSpecialAttack(CardSprite card)
    {
        foreach (int[] distance in AttackRange)
        {
            Field targetField = card.GetTargetField(distance);
            if (targetField == null || !targetField.IsOccupied()) continue;
            targetField.OccupantCard.AdvanceTempStrength(1);
            targetField.OccupantCard.AdvanceTempPower(1);
            targetField.OccupantCard.TakeDamage(card.GetStrength(), card.OccupiedField);
        }
        SkillOnAttack(card);
        return true;
    }

    public override void SkillOnNewTurn(CardSprite card)
    {
        if (!card.OccupiedField.IsAligned(card.Grid.Turn.CurrentAlignment)) return;
        if (attackPause)
        {
            attackPause = false;
            card.BlockAttack();
        }
    }
}
