using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class StaryBert : CharacterConfig
    {
        public StaryBert()
        {
            AddName("stary bert i moze");
            AddSkill(SkillEnum.StaryBert);
            AddProperties(GenderEnum.Male, RoleEnum.Offensive);
            AddStats(1, 2, 5, 3);
            AddRange(0, 2, attackRange);
            AddRange(0, 1, blockRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("212143__qubodup__splash-by-blaukreuz");
        }
    }
}