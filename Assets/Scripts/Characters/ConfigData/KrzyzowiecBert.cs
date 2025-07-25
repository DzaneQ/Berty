﻿using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class KrzyzowiecBert : CharacterConfig
    {
        public KrzyzowiecBert()
        {
            AddName("krzyzowiec bert");
            AddProperties(GenderEnum.Male, RoleEnum.Offensive);
            AddStats(2, 3, 5, 4);
            AddRange(0, 1, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, blockRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, blockRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("733887__velcronator__sword-impact");
        }

        public override void SkillAdjustHealthChange(int value, CardSpriteBehaviour card)
        {
            if (value >= 0) return;
            card.AdvanceStrength(-value, card);
        }
    }
}