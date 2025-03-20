using Berty.CardSprite;
using Berty.Enums;
using Berty.Field;

namespace Berty.Characters.Data
{
    public class RoninBert : Character
    {
        public RoninBert()
        {
            AddName("ronin bert");
            AddProperties(Gender.Male, Role.Offensive);
            AddStats(2, 3, 4, 3);
            AddRange(0, 1, attackRange);
            AddRange(0, 2, attackRange);
            AddRange(0, 1, blockRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("370204__nekoninja__samurai-slash");
        }

        public override bool SkillSpecialAttack(CardSpriteBehaviour card)
        {
            FieldBehaviour[] target = new FieldBehaviour[AttackRange.Count];
            FieldBehaviour lastTarget = null;
            int targetPower = 0;
            for (int i = AttackRange.Count - 1; 0 <= i; i--)
            {
                target[i] = card.GetTargetField(AttackRange[i]);
                if (target[i] == null || !target[i].IsOccupied()) continue;
                lastTarget = target[i];
                break;
            }
            if (lastTarget == null) return true;
            for (int i = 0; i < AttackRange.Count; i++)
            {
                if (target[i] == null || !target[i].IsOccupied()) continue;
                if (target[i].OccupantCard.CardStatus.Power > targetPower) targetPower = target[i].OccupantCard.CardStatus.Power;
                if (target[i] != lastTarget) target[i].OccupantCard.TakeDamage(card.GetStrength(), card.OccupiedField);
                else
                {
                    CardSpriteBehaviour targetCard = lastTarget.OccupantCard;
                    card.SwapWith(lastTarget);
                    targetCard.TakeDamage(card.GetStrength(), card.OccupiedField);
                    break;
                }
            }
            if (targetPower > card.CardStatus.Power) card.AdvanceHealth(-1);
            return true;
        }
    }
}