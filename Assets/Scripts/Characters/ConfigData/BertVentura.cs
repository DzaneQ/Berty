using Berty.BoardCards;
using Berty.Enums;
using System.Collections.Generic;

namespace Berty.BoardCards.ConfigData
{
    public class BertVentura : CharacterConfig
    {
        int opponentNeighborCount;

        public BertVentura()
        {
            AddName("bert ventura");
            AddSkill(SkillEnum.BertVentura);
            AddProperties(GenderEnum.Male, RoleEnum.Offensive);
            AddStats(0, 2, 4, 5);
            AddRange(0, 1, attackRange);
            AddRange(1, 0, attackRange);
            AddRange(-1, 0, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("759965__thekingofgeeks360__parrots-goffins-cockatoo-squawk");
        }
    }
}