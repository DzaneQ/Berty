using Berty.BoardCards;
using Berty.Enums;
using Berty.Grid.Field;

namespace Berty.BoardCards.ConfigData
{
    public class Bertonator : CharacterConfig
    {
        public Bertonator()
        {
            AddName("bertonator");
            AddProperties(GenderEnum.Male, RoleEnum.Agile);
            AddStats(0, 3, 4, 4);
            AddRange(0, 1, attackRange);
            AddRange(0, 1, blockRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("233053__lukeupf__footballtable");
        }

        public override bool SkillSpecialAttack(CardSpriteBehaviour card)
        {
            foreach (int[] distance in AttackRange)
            {
                OutdatedFieldBehaviour targetField = card.GetTargetField(distance);
                if (targetField == null || !targetField.IsOccupied()) continue;
                targetField.OccupantCard.TakeDamage(card.GetStrength(), card.OccupiedField);
                if (!targetField.IsOccupied()) continue;
                targetField.OccupantCard.AdvanceDexterity(-1, card);
                UnityEngine.Debug.Log("Attack - X: " + targetField.GetX() + "; Y: " + targetField.GetY());
                int[] knockback = distance.Clone() as int[];
                knockback[1]++;
                OutdatedFieldBehaviour knockbackField = card.GetTargetField(knockback);
                if (knockbackField == null || knockbackField.IsOccupied()) targetField.OccupantCard.AdvanceHealth(-1);
                else targetField.OccupantCard.SwapWith(knockbackField);
            }
            return true;
        }
    }
}