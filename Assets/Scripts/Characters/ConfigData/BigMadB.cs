using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class BigMadB : CharacterConfig
    {
        public BigMadB()
        {
            AddName("big mad b");
            AddProperties(GenderEnum.Male, RoleEnum.Special);
            AddStats(1, 3, 5, 4);
            AddRange(1, 0, attackRange);
            AddRange(-1, 0, attackRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, blockRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, blockRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("539125__badoink__wobblelooper");
        }

        //public override void SkillOnNewCard(CardSpriteBehaviour card)
        //{
        //    foreach (CharacterConfig character in card.CardManager.AllOutsideCharacters())
        //    {
        //        if (character.Role != RoleEnum.Support) continue;
        //        card.AddResistance(character);
        //    }
        //    foreach (CharacterConfig character in card.Grid.AllInsideCharacters())
        //    {
        //        if (character.Role != RoleEnum.Support) continue;
        //        card.AddResistance(character);
        //    }
        //}

        //public override void SkillOnSuccessfulAttack(CardSpriteBehaviour card)
        //{
        //    card.AdvanceDexterity(-1, card);
        //    card.AdvanceStrength(1);
        //}
    }
}