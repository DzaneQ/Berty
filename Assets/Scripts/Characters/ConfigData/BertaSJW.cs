using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class BertaSJW : CharacterConfig
    {
        public BertaSJW()
        {
            AddName("berta sjw");
            AddSkill(SkillEnum.BertaSJW);
            AddProperties(GenderEnum.Female, RoleEnum.Special);
            AddStats(1, 3, 5, 3);
            AddRange(-1, 0, attackRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, blockRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("418417__wormer2__water-splash-emptying-a-bucket-9");
        }
    }
}