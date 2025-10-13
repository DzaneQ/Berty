using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class BertaTrojanska : CharacterConfig
    {
        public BertaTrojanska()
        {
            AddName("berta trojanska");
            AddSkill(SkillEnum.BertaTrojanska);
            AddProperties(GenderEnum.Female, RoleEnum.Support);
            AddStats(1, 2, 5, 3);
            AddRange(0, 2, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, blockRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, blockRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("706382__agglow__slap");
        }
    }
}