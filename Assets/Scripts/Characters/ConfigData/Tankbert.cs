using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class Tankbert : CharacterConfig
    {
        public Tankbert()
        {
            AddName("tankbert");
            AddSkill(SkillEnum.Tankbert);
            AddProperties(GenderEnum.Male, RoleEnum.Offensive);
            AddStats(5, 5, 2, 5);
            AddRange(0, 1, attackRange);
            AddRange(0, 2, attackRange);
            AddRange(0, 1, blockRange);
            //AddRange(1, 1, riposteRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, blockRange);
            //AddRange(-1, -1, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("182429__qubodup__explosion");
        }
    }
}