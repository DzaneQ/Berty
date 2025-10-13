using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class KowbojBert : CharacterConfig
    {
        public KowbojBert()
        {
            AddName("kowboj bert");
            AddSkill(SkillEnum.KowbojBert);
            AddProperties(GenderEnum.Male, RoleEnum.Agile);
            AddStats(2, 3, 4, 3);
            AddRange(1, 1, attackRange);
            AddRange(-1, 1, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("182577__qubodup__whip-2.8-3.7");
        }
    }
}