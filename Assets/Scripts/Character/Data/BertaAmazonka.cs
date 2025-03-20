public class BertaAmazonka : Character
{
    public BertaAmazonka()
    {
        AddName("berta amazonka");
        AddProperties(Gender.Female, Role.Agile);
        AddStats(2, 3, 4, 3);
        AddRange(0, 1, attackRange);
        AddRange(0, 2, attackRange);
        AddRange(0, 1, riposteRange);
        //AddRange(1, 1, riposteRange);
        //AddRange(1, -1, riposteRange);
        AddRange(0, -1, riposteRange);
        //AddRange(-1, -1, riposteRange);
        //AddRange(-1, 1, riposteRange);
        AddSoundEffect("347884__arcandio__razorback-archery-66-66.6");
    }

    public override bool SkillSpecialAttack(CardSpriteBehaviour card)
    {
        foreach (int[] distance in card.Character.AttackRange)
        {
            Field targetField = card.GetTargetField(distance);
            if (targetField == null || !targetField.IsOccupied()) continue;
            targetField.OccupantCard.TakeDamage(card.GetStrength(), card.OccupiedField);
            int[] neighbor = distance.Clone() as int[];
            neighbor[0]--;
            targetField = card.GetTargetField(neighbor);
            if (targetField != null && targetField.IsOccupied()) targetField.OccupantCard.AdvanceHealth(-1);
            neighbor[0] = neighbor[0] + 2;
            targetField = card.GetTargetField(neighbor);
            if (targetField != null && targetField.IsOccupied()) targetField.OccupantCard.AdvanceHealth(-1);
        }
        return true;
    }
}
