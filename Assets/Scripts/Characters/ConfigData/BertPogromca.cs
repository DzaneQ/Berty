using Berty.BoardCards;
using Berty.Enums;
using Berty.Grid.Field;

namespace Berty.BoardCards.ConfigData
{
    public class BertPogromca : CharacterConfig
    {
        public BertPogromca()
        {
            AddName("bert pogromca");
            AddSkill(SkillEnum.BertPogromca);
            AddProperties(GenderEnum.Male, RoleEnum.Offensive);
            AddStats(2, 3, 4, 4);
            AddRange(0, 2, attackRange);
            AddRange(-1, 1, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, blockRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("536528__smice_6__chop-off-head-with-axe");
        }
    }
}