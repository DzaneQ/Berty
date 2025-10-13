using Berty.BoardCards;
using Berty.Enums;
using Berty.Grid.Field;

namespace Berty.BoardCards.ConfigData
{
    public class RycerzBerti : CharacterConfig
    {
        public RycerzBerti()
        {
            AddName("rycerz berti");
            AddSkill(SkillEnum.RycerzBerti);
            AddProperties(GenderEnum.Female, RoleEnum.Support);
            AddStats(2, 4, 5, 4);
            AddRange(0, 2, attackRange);
            AddRange(1, 0, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, blockRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("638292__captainyulef__laser-sword");
        }
    }
}