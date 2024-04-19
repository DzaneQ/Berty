using System.Collections.Generic;

public class BigMadB : Character
{
    public BigMadB()
    {
        AddName("big mad b");
        AddProperties(Gender.Male, Role.Special);
        AddStats(1, 3, 5, 4);
        AddRange(1, 0, attackRange);
        AddRange(-1, 0, attackRange);
        //AddRange(1, 1, riposteRange);
        AddRange(1, 0, blockRange);
        //AddRange(1, -1, riposteRange);
        //AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, blockRange);
        //AddRange(-1, 1, riposteRange);
    }

    public override void SkillOnNewCard(CardSprite card)
    {
        foreach (Character character in card.CardManager.AllOutsideCharacters())
        {
            if (character.Role != Role.Support) continue;
            card.AddResistance(character);
        }
        foreach (Character character in card.Grid.AllInsideCharacters())
        {
            if (character.Role != Role.Support) continue;
            card.AddResistance(character);
        }
    }

    public override void SkillOnSuccessfulAttack(CardSprite card)
    {
        card.AdvanceDexterity(-1, card);
        card.AdvanceStrength(1);
    }
}
