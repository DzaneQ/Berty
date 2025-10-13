using Berty.BoardCards;
using Berty.Enums;
using Berty.Grid.Field;

namespace Berty.BoardCards.ConfigData
{
    public class Zombert : CharacterConfig
    {
        public Zombert()
        {
            AddName("zombert");
            AddSkill(SkillEnum.Zombert);
            AddProperties(GenderEnum.Male, RoleEnum.Offensive);
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
    }
}