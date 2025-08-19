using Berty.BoardCards;
using Berty.Enums;
using Berty.Grid.Field;

namespace Berty.BoardCards.ConfigData
{
    public class BertPogromca : CharacterConfig
    {
        public BertPogromca()
        {
            AddName("bert pogromca");
            AddProperties(GenderEnum.Male, RoleEnum.Offensive);
            AddStats(2, 3, 4, 4);
            AddRange(0, 2, attackRange);
            AddRange(-1, 1, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, blockRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("536528__smice_6__chop-off-head-with-axe");
        }

        //public override void SkillOnNewCard(CardSpriteBehaviour card)
        //{
        //    foreach (CharacterConfig character in card.CardManager.AllOutsideCharacters())
        //    {
        //        if (character.Role != RoleEnum.Special) continue;
        //        card.AddResistance(character);
        //    }
        //    foreach (CharacterConfig character in card.Grid.AllInsideCharacters())
        //    {
        //        if (character.Role != RoleEnum.Special) continue;
        //        card.AddResistance(character);
        //    }
        //}

        //public override bool SkillSpecialAttack(CardSpriteBehaviour card)
        //{
        //    foreach (int[] distance in AttackRange)
        //    {
        //        OutdatedFieldBehaviour targetField = card.GetTargetField(distance);
        //        if (targetField == null || !targetField.IsOccupied()) continue;
        //        if (targetField.OccupantCard.Character.Role == RoleEnum.Special)
        //            targetField.OccupantCard.TakeDamage(card.GetStrength() + 2, card.OccupiedField);
        //        else targetField.OccupantCard.TakeDamage(card.GetStrength(), card.OccupiedField);
        //        //Debug.Log("Attack - X: " + targetField.GetX() + "; Y: " + targetField.GetY());
        //    }
        //    return true;
        //}

        //public override bool GlobalSkillResistance() => true;
    }
}