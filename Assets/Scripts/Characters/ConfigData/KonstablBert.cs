using Berty.BoardCards;
using Berty.Enums;
using Berty.Grid;
using Berty.Grid.Field;

namespace Berty.BoardCards.ConfigData
{
    public class KonstablBert : CharacterConfig
    {
        public KonstablBert()
        {
            AddName("konstabl bert");
            AddProperties(GenderEnum.Male, RoleEnum.Agile);
            AddStats(2, 3, 5, 3);
            AddRange(0, 2, attackRange);
            //AddRange(1, 1, riposteRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, blockRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, blockRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("577354__thecrow_br__club");
        }

        public override void SkillOnNewCard(CardSpriteBehaviour card)
        {
            foreach (OutdatedFieldBehaviour field in card.Grid.Fields)
            {
                if (!field.IsOccupied()) continue;
                if (field.OccupantCard.GetRole() == RoleEnum.Special) field.OccupantCard.AdvanceHealth(-1);
            }
        }

        public override bool SkillSpecialAttack(CardSpriteBehaviour card)
        {
            foreach (int[] distance in AttackRange)
            {
                OutdatedFieldBehaviour targetField = card.GetTargetField(distance);
                if (targetField == null || !targetField.IsOccupied()) continue;
                switch (targetField.OccupantCard.GetRole())
                {
                    case RoleEnum.Special:
                    case RoleEnum.Support:
                        targetField.OccupantCard.TakeDamage(card.GetStrength() + 1, card.OccupiedField);
                        break;
                    default:
                        targetField.OccupantCard.TakeDamage(card.GetStrength(), card.OccupiedField);
                        break;
                }
                UnityEngine.Debug.Log("Attack - X: " + targetField.GetX() + "; Y: " + targetField.GetY());
            }
            return true;
        }
    }
}