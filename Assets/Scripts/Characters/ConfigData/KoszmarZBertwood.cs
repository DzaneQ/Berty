using Berty.BoardCards;
using Berty.Enums;
using Berty.Grid.Field;

namespace Berty.BoardCards.ConfigData
{
    public class KoszmarZBertwood : CharacterConfig
    {
        private bool attackPause;

        public KoszmarZBertwood()
        {
            AddName("koszmar z bertwood");
            AddProperties(GenderEnum.Kid, RoleEnum.Special);
            AddStats(3, 5, 3, 5);
            AddRange(0, 1, attackRange);
            AddRange(1, 0, attackRange);
            AddRange(1, -1, attackRange);
            AddRange(0, -1, attackRange);
            AddRange(-1, -1, attackRange);
            AddRange(-1, 0, attackRange);
            AddRange(-1, 1, attackRange);
            AddRange(0, 1, blockRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, blockRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, blockRange);
            //AddRange(-1, -1, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("564485__rizzard__monster-growl");
        }

        //public override void SkillOnNewCard(CardSpriteBehaviour card)
        //{
        //    attackPause = false;
        //}

        //public override void SkillOnAttack(CardSpriteBehaviour card)
        //{
        //    attackPause = true;
        //}

        //public override bool SkillSpecialAttack(CardSpriteBehaviour card)
        //{
        //    foreach (int[] distance in AttackRange)
        //    {
        //        OutdatedFieldBehaviour targetField = card.GetTargetField(distance);
        //        if (targetField == null || !targetField.IsOccupied()) continue;
        //        targetField.OccupantCard.AdvanceTempStrength(1, card);
        //        targetField.OccupantCard.AdvanceTempPower(1, card);
        //        targetField.OccupantCard.TakeDamage(card.GetStrength(), card.OccupiedField);
        //    }
        //    SkillOnAttack(card);
        //    return true;
        //}

        //public override void SkillOnNewTurn(CardSpriteBehaviour card)
        //{
        //    if (!card.OccupiedField.IsAligned(card.Grid.Turn.CurrentAlignment)) return;
        //    if (attackPause)
        //    {
        //        attackPause = false;
        //        card.ExhaustAttack();
        //    }
        //}
    }
}