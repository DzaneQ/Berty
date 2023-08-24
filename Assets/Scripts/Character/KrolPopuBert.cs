public class KrolPopuBert : Character
{
    public KrolPopuBert()
    {
        AddName("krol popu bert");
        AddProperties(Gender.Male, Role.Special);
        AddStats(1, 3, 5, 2);
        AddRange(0, 1, attackRange);
        AddRange(1, 2, attackRange);
        AddRange(-1, 2, attackRange);
        AddRange(0, 1, riposteRange);
        //AddRange(1, 1, riposteRange);
        AddRange(1, 0, riposteRange);
        //AddRange(1, -1, riposteRange);
        //AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        //AddRange(-1, 1, riposteRange);
    }

    public override void SkillAdjustHealthChange(int value, CardSprite card)
    {
        if (card.CardStatus.Health > 0) return;
        Character kid = null;
        foreach (Character character in card.CardManager.AllOutsideCharacters())
        {
            if (character.Gender != Gender.Kid) continue;
            //if (IsEnemyCard(character, card)) continue;
            kid = character;
            break;
        }
        if (kid == null) return;

    }

    //private bool IsEnemyCard(Character character, CardSprite card)
    //{
    //    if (card.Grid.Turn.CurrentAlignment == card.CardManager.EnabledCards.Contains)
    //}
}
