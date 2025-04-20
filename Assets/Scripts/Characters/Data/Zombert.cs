using Berty.CardSprite;
using Berty.Enums;
using Berty.Field;

namespace Berty.Characters.Data
{
    public class Zombert : CharacterConfig
    {
        public Zombert()
        {
            AddName("zombert");
            AddProperties(Gender.Male, Role.Offensive);
            AddStats(1, 2, 4, 3);
            AddRange(0, 1, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("400174__jgriffie919__flesh-bite");
        }

        public override void SkillOnSuccessfulAttack(CardSpriteBehaviour card)
        {
            FieldBehaviour targetField = card.GetTargetField(AttackRange[0]);
            if (!targetField.IsOccupied() || card.IsAllied(targetField)) return;
            targetField.OccupantCard.AdvancePower(-1, card);
        }

        public override void SkillOnOtherCardDeath(CardSpriteBehaviour card, CardSpriteBehaviour otherCard)
        {
            if (card.IsAllied(otherCard.OccupiedField)) return;
            card.AdvanceHealth(1);
        }
    }
}