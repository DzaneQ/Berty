using Berty.BoardCards;
using Berty.Enums;
using System.Collections.Generic;

namespace Berty.BoardCards.ConfigData
{
    public class ZalobnyBert : CharacterConfig
    {
        public ZalobnyBert()
        {
            AddName("zalobny bert");
            SetCharacter(CharacterEnum.ZalobnyBert);
            AddProperties(GenderEnum.Male, RoleEnum.Special);
            AddStats(0, 3, 3, 4);
            AddRange(0, 1, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("474266__mkzing__bell-with-crows-6.1-8.3");
        }

        //public override int SkillDefenceModifier(int damage, CardSpriteBehaviour attacker)
        //{
        //    if (attacker.GetRole() == RoleEnum.Offensive) return 0;
        //    return damage;
        //}

        //public override void SkillAdjustHealthChange(int value, CardSpriteBehaviour card)
        //{
        //    if (0 <= value) return;
        //    foreach (CardSpriteBehaviour adjCard in card.GetAdjacentCards())
        //        if (card.IsAllied(adjCard.OccupiedField))
        //            adjCard.AdvanceHealth(-2 * value);
        //}
    }
}