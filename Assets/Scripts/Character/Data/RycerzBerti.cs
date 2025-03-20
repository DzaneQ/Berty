public class RycerzBerti : Character
{
    public RycerzBerti()
    {
        AddName("rycerz berti");
        AddProperties(Gender.Female, Role.Support);
        AddStats(2, 4, 5, 4);
        AddRange(0, 2, attackRange);
        AddRange(1, 0, attackRange);
        AddRange(0, 1, riposteRange);
        //AddRange(1, 1, riposteRange);
        AddRange(1, 0, blockRange);
        //AddRange(1, -1, riposteRange);
        AddRange(0, -1, riposteRange);
        //AddRange(-1, -1, riposteRange);
        //AddRange(-1, 1, riposteRange);
        AddSoundEffect("638292__captainyulef__laser-sword");
    }

    public override void SkillOnNewCard(CardSpriteBehaviour card)
    {
        Field targetField = null;
        for (int i = 0; i < 4; i++)
        {
            Field adjacentField = card.GetAdjacentField(i * 90);
            if (adjacentField == null || !adjacentField.IsOccupied()) continue;
            targetField = adjacentField;
            if (adjacentField.IsOpposed(card.OccupiedField.Align)) break;
        }
        if (targetField != null) targetField.OccupantCard.AdvancePower(-3, card);
        card.Grid.SetTelekinesis(card.OccupiedField.Align, card.CardStatus.Dexterity);
    }

    public override void SkillAdjustDexterityChange(int value, CardSpriteBehaviour card)
    {
        card.Grid.SetTelekinesis(card.OccupiedField.Align, card.CardStatus.Dexterity);
    }

    public override void SkillOnDeath(CardSpriteBehaviour card)
    {
        card.Grid.RemoveTelekinesis();
    }
}
