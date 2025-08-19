using Berty.BoardCards;
using Berty.Enums;
using Berty.Grid.Field;

namespace Berty.BoardCards.ConfigData
{
    public class RoninBert : CharacterConfig
    {
        public RoninBert()
        {
            AddName("ronin bert");
            AddProperties(GenderEnum.Male, RoleEnum.Offensive);
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

        //public override bool SkillSpecialAttack(CardSpriteBehaviour card)
        //{
        //    OutdatedFieldBehaviour[] target = new OutdatedFieldBehaviour[AttackRange.Count];
        //    OutdatedFieldBehaviour lastTarget = null;
        //    int targetPower = 0;
        //    for (int i = AttackRange.Count - 1; 0 <= i; i--)
        //    {
        //        target[i] = card.GetTargetField(AttackRange[i]);
        //        if (target[i] == null || !target[i].IsOccupied()) continue;
        //        lastTarget = target[i];
        //        break;
        //    }
        //    if (lastTarget == null) return true;
        //    for (int i = 0; i < AttackRange.Count; i++)
        //    {
        //        if (target[i] == null || !target[i].IsOccupied()) continue;
        //        if (target[i].OccupantCard.CardStatus.Power > targetPower) targetPower = target[i].OccupantCard.CardStatus.Power;
        //        if (target[i] != lastTarget) target[i].OccupantCard.TakeDamage(card.GetStrength(), card.OccupiedField);
        //        else
        //        {
        //            CardSpriteBehaviour targetCard = lastTarget.OccupantCard;
        //            card.SwapWith(lastTarget);
        //            targetCard.TakeDamage(card.GetStrength(), card.OccupiedField);
        //            break;
        //        }
        //    }
        //    if (targetPower > card.CardStatus.Power) card.AdvanceHealth(-1);
        //    return true;
        //}
    }
}