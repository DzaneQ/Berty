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
            AddSkill(SkillEnum.RoninBert);
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
    }
}